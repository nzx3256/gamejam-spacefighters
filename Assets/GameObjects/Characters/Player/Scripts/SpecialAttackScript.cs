using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public abstract class SpecialAttackScript : MonoBehaviour
{
    public bool activated;

    protected virtual void EnableVisuals(bool enable)
    {
    }
}
