using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System;
using System.Linq;
using Unity.VisualScripting;
using NUnit.Framework.Internal;
using Unity.Mathematics;
using UnityEngine.UIElements;
using System.Linq.Expressions;

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
    [SerializeField] [Tooltip("Move spawner based on offset to camera when spawning objects")]
    private bool moveWhenSpawning = true;
    [SerializeField]
    private float moveTimeDelay = 5f;
    private bool move = false;
    [SerializeField]
    protected SplineDirection direction;
    [SerializeField]
    private List<EnemySpawnGroupWrapper> SpawnOrder;

    protected int spawnCounts = 0;

    private Vector3 offsetToCam;

    protected IEnumerator SpawnObjectGroups()
    {
        _spawning = true;
        OnSpawn();
        foreach (EnemySpawnGroupWrapper wrapper in SpawnOrder)
        {
            for (int i = 0; i < wrapper.SpawnGroup.numToSpawn; i++)
            {
                if(wrapper.splineAnimate == null)
                {
                    break;
                }
                Vector3 spawnPosition = transform.position;
                if (wrapper.splineAnimate.gameObject.scene.name != null) //if gameObject wrapper is not a prefab
                {
                    spawnPosition = wrapper.splineAnimate.gameObject.transform.position;
                }
                GameObject obj = Instantiate(wrapper.splineAnimate.gameObject, spawnPosition, Quaternion.identity);
                SplineAnimate splComp;
                if (!obj.TryGetComponent(out splComp))
                {
                    Debug.LogError("No SplineAnimate Component on Prefab " + wrapper.splineAnimate.gameObject.name);
                    break;
                }
                StartCoroutine(moveToSplineThenPlay(splComp, wrapper.moveSpline));
                // splComp.PlayOnAwake = true;
                // splComp.Container = wrapper.moveSpline;
                // splComp.Play();

                yield return new WaitForSeconds(wrapper.SpawnGroup.delay);
            }
        }
        _spawning = false;
        spawnCounts++;
    }

    private IEnumerator moveToSplineThenPlay(SplineAnimate comp, SplineContainer spline)
    {
        Vector2 target = transform.position;
        if (spline != null)
        {
            float3 temp = spline.EvaluatePosition(0);
            target = new Vector2(temp.x, temp.y);
        }
        while (Vector2.Distance((Vector2)comp.transform.position , target) > 0.1f)
        {
            comp.transform.position = Vector2.Lerp(comp.transform.position, target, 0.1f);
            yield return new WaitForFixedUpdate();
        }
        if (spline != null)
        {
            comp.Container = spline;
            comp.Play();
        }
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
        if(move || _spawning)
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
        move = move ? false : true;
        yield return new WaitForSeconds(moveTimeDelay);
        move = move ? false : true;
    }
}
