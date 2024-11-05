using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System;
using System.Linq;

public class BaseSpawner : MonoBehaviour
{
    protected bool _spawning = false;
    public bool spawning 
    {
        get => _spawning;
    }
    protected enum SplineDirection
    {
        Left=0,
        Right = 1
    }
    [SerializeField] [Tooltip("Move spawner based on camera offset when spawning objects")]
    private bool moveWhenSpawning = true;
    [SerializeField]
    private float moveTimeDelay;
    private bool move = false;
    [SerializeField]
    protected SplineDirection direction;
    [SerializeField]
    private List<SpawnerGroup> SpawnOrder;

    protected int spawnCounts = 0;

    private Vector3 offsetToCam;

    protected IEnumerator SpawnObjectGroups()
    {
        _spawning = true;
        OnSpawn();
        foreach(SpawnerGroup group in SpawnOrder)
        {
            for(int i=0;i<group.NumToSpawn;i++)
            {
                GameObject obj = Instantiate(group.splineAnimate.gameObject, transform.position, Quaternion.identity);
                SplineAnimate splComp = obj.GetComponent<SplineAnimate>();
                splComp.PlayOnAwake = true;
                splComp.Container = group.MoveSpline;
                splComp.Play();

                yield return new WaitForSeconds(group.Delay);
            }
        }
        _spawning = false;
        spawnCounts++;
    }

    protected virtual void Start()
    {
        if(direction == SplineDirection.Right)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    protected virtual void Update()
    {
        if(move && _spawning)
        {
            //Moves the spawn based on camera position by default.
            transform.position = Camera.main.transform.position + offsetToCam;
        }
    }

    protected virtual void OnSpawn()
    {
        if(moveWhenSpawning)
        {
            offsetToCam = (transform.position - Camera.main.transform.position);
            StartCoroutine(moveToggleCoroutine());
        }
    }

    private IEnumerator moveToggleCoroutine()
    {
        move = true;
        yield return new WaitForSeconds(moveTimeDelay);
        move = false;
    }
}
