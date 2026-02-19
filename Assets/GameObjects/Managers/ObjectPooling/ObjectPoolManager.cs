using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;
    public static ObjectPoolManager instance
    {
        get => _instance;
    }

    [SerializeField]
    private List<OPM_InitElement> initializerList;
    [SerializeField]
    private bool throwExceptions = true;
    private Dictionary<PoolData, ObjectPool<GameObject>> _objectPoolReferences;

    public Dictionary<PoolData, ObjectPool<GameObject>> ObjectPoolReferences
    {
        get => _objectPoolReferences;
    }

    private static int counter = 0;

    //If Domain and Scene Reloading are turned on
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticData()
    {
        counter = 0;
        _instance = null;
    }


    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _objectPoolReferences = new Dictionary<PoolData, ObjectPool<GameObject>>();
        foreach (var element in initializerList)
        {
            IPoolable poolable;
            GameObject obj = element.PooledGameObject;
            if(obj.scene.name == null)
            {
                obj = Instantiate(obj, new UnityEngine.Vector3(0f,0f,10f), Quaternion.identity);
                obj.SetActive(false);
            }
            if(obj.TryGetComponent(out poolable))
            {
                if(poolable.poolData == null)
                {
                    poolable.poolData = (PoolData)ScriptableObject.CreateInstance(typeof(PoolData));
                    poolable.poolData.ObjectID = counter++;
                }
                if(!_objectPoolReferences.ContainsKey(poolable.poolData))
                {
                    var pool = new ObjectPool<GameObject>(
                        poolable.CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        throwExceptions,
                        element.InitialPoolSize,
                        element.PoolMaxCapacity);
                    _objectPoolReferences.Add(poolable.poolData, pool);
                }
            }
        }
    }
    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void OnDestroyPoolObject(GameObject obj)
    {
        GameObject.DestroyImmediate(obj);
    }

    public ObjectPool<GameObject> GetObjectPool(PoolData data)
    {

        return _objectPoolReferences[data];
    }

    public bool TryGetObjectPool(PoolData data, out ObjectPool<GameObject> pool)
    {
        pool = null;
        if(data == null)
        {
            return false;
        }
        if (_objectPoolReferences.ContainsKey(data))
        {
            pool = GetObjectPool(data);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckThenTryGetObjectPool(PoolData data, out ObjectPool<GameObject> pool)
    {
        pool = null;
        ObjectPoolManager opm_inst;
        if(ObjectPoolManager.instance != null)
        {
            opm_inst = ObjectPoolManager.instance;
        }
        else
        {
            return false;
        }
        if(data == null)
        {
            return false;
        }
        if (opm_inst.ObjectPoolReferences.ContainsKey(data))
        {
            pool = opm_inst.GetObjectPool(data);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool SetInScene()
    {
        if(ObjectPoolManager.instance != null)
        {
            return true;
        }
        return false;
    }
}
