using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BulletSpawnGroupWrapper
{
    [SerializeField]
    private SpawnerGroup spawnGroup;
    /*[SerializeField]
    private BulletScript _bulletScript;*/
    [SerializeField]
    private UnityEvent _fireAction;

    public SpawnerGroup SpawnGroup
    { get => spawnGroup; }
    /*public BulletScript bulletScript 
    { get => _bulletScript; }*/
    public UnityEvent fireAction
    { get => _fireAction; }
}
