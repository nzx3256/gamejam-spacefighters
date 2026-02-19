using UnityEngine;

[System.Serializable]
public class OPM_InitElement
{
    [SerializeField]
    private GameObject pooledGameObject;
    [SerializeField]
    private int initialPoolSize;
    [SerializeField]
    private int poolMaxCapacity;

    public GameObject PooledGameObject
    {
        get => pooledGameObject;
    }

    public int InitialPoolSize
    {
        get => initialPoolSize;
    }

    public int PoolMaxCapacity
    {
        get => poolMaxCapacity;
    }
}