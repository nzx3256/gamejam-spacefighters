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
            transform.position = boundingBox.position + new Vector3(0,0,-10);
        }
    }
}
