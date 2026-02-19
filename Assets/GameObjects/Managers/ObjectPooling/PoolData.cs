using UnityEngine;

public class PoolData : ScriptableObject
{
    public int ObjectID;

    public bool Equals(PoolData data)
    {
        return ObjectID == data.ObjectID;
    }
}