using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour, IPoolable
{
    [SerializeField]
    protected PoolData _poolData;
    public PoolData poolData
    {
        get => _poolData;
        set 
        {
            if (_poolData == null)
                _poolData = value;
        }
    }

    protected ObjectPool<GameObject> pool = null;

    protected virtual void OnEnable()
    {
        if(!ObjectPoolManager.CheckThenTryGetObjectPool(_poolData, out pool))
        {
            ;
        }
    }

    protected void Update()
    {
    }

    public void ReleaseOrDestroy()
    {
        if(pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreatePooledItem()
    {
        GameObject obj = Instantiate(gameObject, transform.position, Quaternion.identity);
        obj.SetActive(false);
        return obj;
    }
}
