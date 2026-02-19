using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProximitySpawner : BaseSpawner
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private float spawnDistance;

    private ObjectPool<GameObject> pool;

    sealed protected override void Update()
    {
        //base.Update();
        if(targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
        if(!_spawning && 
            Vector2.Distance(
                (Vector2)targetTransform.position, 
                (Vector2)transform.position) <= spawnDistance) {
            if(spawnCounts == 0)
            {
                StartCoroutine(SpawnObjectGroups());
            }
        }
    }
}
