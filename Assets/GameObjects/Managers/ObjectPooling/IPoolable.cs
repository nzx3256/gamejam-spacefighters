using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable
{
    public PoolData poolData
    {
        get;
        set;
    }
    public GameObject CreatePooledItem();


    /*
    public ObjectPool<GameObject> ObjectPool
    {
        get;
        set;
    }
    /*
    public bool CollectionCheck
    {
        get;
    }
    public int DefaultCapacity
    {
        get;
    }
    public int MaxPoolSize
    {
        get;
    }
    /*
    public GameObject CreatePooledItem();
    public void OnTakeFromPool(GameObject poolable);
    public void OnReturnedToPool(GameObject poolable);
    public void OnDestroyPoolObject(GameObject poolable);
    */

// createFunc	Used to create a new instance when the pool is empty. In most cases this will just be () => new T().
// actionOnGet	Called when the instance is taken from the pool.
// actionOnRelease	Called when the instance is returned to the pool. This can be used to clean up or disable the instance.
// actionOnDestroy	Called when the element could not be returned to the pool due to the pool reaching the maximum size.
// collectionCheck	Collection checks are performed when an instance is returned back to the pool. An exception will be thrown if the instance is already in the pool. Collection checks are only performed in the Editor.
// defaultCapacity	The default capacity the stack will be created with.
// maxSizeThe   maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size. 
}