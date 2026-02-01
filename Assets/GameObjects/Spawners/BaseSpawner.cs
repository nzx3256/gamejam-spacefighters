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
using UnityEngine.Events;

public class BaseSpawner : MonoBehaviour
{
    protected bool _spawning = false;
    public bool spawning 
    {
        get => _spawning;
    }

    // protected enum SplineMovementType
    // {
    //     TIMED,
    //     FOREVER,
        
    // }
    // [SerializeField] [Tooltip("Move spawner based on offset to camera when spawning objects")]
    // private bool moveWhenSpawning = true;
    // private float moveTimeDelay = 5f;
    // private bool move = false;
    [SerializeField]
    private List<EnemySpawnGroupWrapper> SpawnOrder;

    protected int spawnCounts = 0;

    public UnityEvent OnSpawningEvent;

    protected IEnumerator SpawnObjectGroups()
    {
        _spawning = true;
        // OnSpawn();
        OnSpawningEvent?.Invoke();
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
                obj.SetActive(true);
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
        
    private Vector2 getSplinePosition(SplineContainer spline, float w)
    {
        if(spline != null)
        {
            float3 temp = spline.EvaluatePosition(w);
            return new Vector2(temp.x, temp.y);
        }
        else
        {
            return (Vector2)transform.position;
        }
    }

    private IEnumerator moveToSplineThenPlay(SplineAnimate comp, SplineContainer spline)
    {
        Collider2D col;
        if(comp.gameObject.TryGetComponent(out col))
        {
            col.enabled = false;
        }
        Vector2 target = getSplinePosition(spline,comp.StartOffset);
        while (Vector2.Distance((Vector2)comp.transform.position , target) > 0.2f)
        {
            target = getSplinePosition(spline,comp.StartOffset);
            comp.transform.position = Vector2.MoveTowards(comp.transform.position, target, comp.MaxSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        if(col != null)
        {
            col.enabled = true;
        }
        if (spline != null)
        {
            comp.Container = spline;
            comp.Play();
        }
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        // if(move || _spawning)
        // {
        //     //Moves the spawn based on camera position by default.
        //     transform.position = Camera.main.transform.position + offsetToCam;
        // }
    }

}
