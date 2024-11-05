using System;
using System.Collections;
using UnityEngine;

public class TimedSpawner : BaseSpawner
{
    [SerializeField]
    private float timeInSeconds = 0;

    private Coroutine timerCoroutine = null;

    sealed protected override void Start()
    {
        if(timeInSeconds > 0)
        {
            timerCoroutine = StartCoroutine(SpawnTimer());
        }
        else
        {
            Debug.LogError("Time in Seconds is unset. Will not be starting the timer");
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(timeInSeconds);
        StartCoroutine(SpawnObjectGroups());
        timerCoroutine = null;
    }
}
