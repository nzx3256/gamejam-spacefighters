using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class EnemySpawnGroupWrapper
{
    [SerializeField]
    private SpawnerGroup spawnGroup;
    [SerializeField]
    private SplineContainer _moveSpline;
    [SerializeField]
    private SplineAnimate _splineAnimate;

    public SpawnerGroup SpawnGroup
    { get => spawnGroup; }
    public SplineContainer moveSpline
    { get => _moveSpline; }
    public SplineAnimate splineAnimate
    { get => _splineAnimate; }
}
