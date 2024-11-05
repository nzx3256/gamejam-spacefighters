using System;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class SpawnerGroup
{
    [SerializeField]
    private SplineAnimate splAnim;
    [SerializeField]
    private int numToSpawn = 1;
    [SerializeField]
    private SplineContainer moveSpline;
    [SerializeField] [Range(0.3f,10f)]
    private float delay = 0.5f;

    public SplineAnimate splineAnimate
    {
        get => splAnim;
    }
    public int NumToSpawn
    {
        get => numToSpawn;
    }
    public SplineContainer MoveSpline
    {
        get => moveSpline;
    }
    public float Delay
    {
        get => delay;
    }

    public SpawnerGroup(SplineAnimate sAnim, SplineContainer spl, int num = 1, float d = 0.5f)
    {
        splAnim = sAnim;
        numToSpawn = num;
        moveSpline = spl;
        delay = d;
    }
}
