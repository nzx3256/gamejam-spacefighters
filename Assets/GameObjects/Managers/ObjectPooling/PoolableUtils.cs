using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PoolableUtils
{
    public static GameObject GetFromPoolOrInstantiate(GameObject obj, Vector3 spawnPosition, Quaternion rotation)
    {
        IPoolable poolable;
        ObjectPool<GameObject> objectPool;
        if( obj.TryGetComponent(out poolable) && 
            ObjectPoolManager.CheckThenTryGetObjectPool(poolable.poolData, out objectPool))
        {
            obj = objectPool.Get();
            obj.transform.position = spawnPosition;
            obj.transform.rotation = rotation;
        }
        else 
        {
            obj = GameObject.Instantiate(obj, spawnPosition, rotation);
            obj.SetActive(true);
        }
        return obj;
    }

    public static GameObject GetFromPoolOrInstantiate(GameObject obj, Vector3 spawnPosition)
    {
        return PoolableUtils.GetFromPoolOrInstantiate(obj, spawnPosition, Quaternion.identity);
    }

    public static void ReleaseToPoolOrDestroy(GameObject obj)
    {
        IPoolable poolable;
        ObjectPool<GameObject> objectPool;
        if( obj.TryGetComponent(out poolable) &&
            ObjectPoolManager.CheckThenTryGetObjectPool(poolable.poolData, out objectPool))
        {
            obj.transform.position = Vector2.zero;
            objectPool.Release(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
}
