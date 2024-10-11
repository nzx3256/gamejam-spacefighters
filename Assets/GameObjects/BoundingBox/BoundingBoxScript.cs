using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;
    private float camSize;
    [SerializeField]
    private float moveSpeed = 1f;
    private Transform up, down, left, right;

    private void Start()
    {
        cam = Camera.main;
        if(cam == null || !cam.orthographic)
        {
            Debug.Log("No orthographic camera in scene. Disabling this script: ["+gameObject.name+","+this.name+"]");
            this.enabled = false;
        }
        else
        {
            camSize = cam.orthographicSize;
        }
        if(!TryGetComponent(out rb))
        {
            Debug.Log("Rigidbody2Dd not set on " + gameObject.name + ".");
        }

        InitColliders();

    }

    private void InitColliders()
    {
        float aspect = cam.aspect;
        Debug.Log("\nRatio: " + cam.aspect + "\nResolution: " + Screen.currentResolution.width + ":" + Screen.currentResolution.height);
        
        foreach(Transform child in transform)
        {
            switch(child.name)
            {
                case "up":
                    up = child;
                    break;
                case "down":
                    down = child;
                    break;
                case "left":
                    left = child;
                    break;
                case "right":
                    right = child;
                    break;
                default:
                    Debug.Log("WARNING: Child in \"" + gameObject.name + "\" with name other than up, down, left, or right.");
                    break;
            }
        }

        up.position = cam.transform.position + camSize * Vector3.up;
        down.position = cam.transform.position + camSize * Vector3.down;
        left.position = cam.transform.position + camSize * aspect * Vector3.left;
        right.position = cam.transform.position + camSize * aspect * Vector3.right;
        up.GetComponent<BoxCollider2D>().size = new Vector2(2*camSize*aspect,camSize*0.3f);
        down.GetComponent<BoxCollider2D>().size = new Vector2(2*camSize*aspect,camSize*0.3f);
        left.GetComponent<BoxCollider2D>().size = new Vector2(0.3f*camSize,camSize*2);
        right.GetComponent<BoxCollider2D>().size = new Vector2(0.3f*camSize,camSize*2);
    }

    void Update()
    {
        try{
            //while the game is playing
            if(rb != null)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, transform.up * moveSpeed, 0.5f);
            }
            if(camSize != cam.orthographicSize)
            {
                InitColliders();
                camSize = cam.orthographicSize;
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }
}
