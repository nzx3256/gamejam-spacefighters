using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private List<GameObject> bullets;

    [SerializeField]
    public bool moveRelativeToTarget;
    [SerializeField]
    private GameObject followTarget;

    void Start()
    {
    }

    void Update()
    {
    }
}
