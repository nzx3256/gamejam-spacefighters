using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private float moveVelocity = 1f;

    public float upwardVelocity
    {
        get => moveVelocity;
    }
    void Start()
    {
    }

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * moveVelocity;
    }

    private float getDeltaY()
    {
        return moveVelocity;
    }
}
