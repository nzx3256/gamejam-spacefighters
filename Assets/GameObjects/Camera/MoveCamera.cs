using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Transform boundingBox;
    
    void Start()
    {
        boundingBox = FindObjectOfType<BoundingBoxScript>().transform;
    }

    void Update()
    {
        if(boundingBox != null)
        {
            transform.position = (Vector3)Vector2.MoveTowards((Vector2)transform.position, (Vector2)boundingBox.position, 0.5f*Time.deltaTime) + new Vector3(0,0,-10);
        }
    }
}
