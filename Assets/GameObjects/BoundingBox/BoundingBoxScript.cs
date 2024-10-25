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
    private Transform up, down, left, right, player1Spawn, player2Spawn;

    private void Start()
    {
        cam = Camera.main;
        if(cam == null || !cam.orthographic)
        {
            //Debug.Log("No orthographic camera in scene. Disabling this script: ["+gameObject.name+","+this.name+"]");
            this.enabled = false;
        }
        else
        {
            camSize = cam.orthographicSize;
        }
        if(!TryGetComponent(out rb))
        {
            //Debug.Log("Rigidbody2Dd not set on " + gameObject.name + ".");
        }

        _InitChildren();

    }

    private void _InitChildren()
    {
        float aspect = cam.aspect;
        //Debug.Log("\nRatio: " + cam.aspect + "\nResolution: " + Screen.currentResolution.width + ":" + Screen.currentResolution.height);
        
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
                case "Player1Spawn":
                    player1Spawn = child;
                    break;
                case "Player2Spawn":
                    player2Spawn = child;
                    break;
                default:
                    //Debug.Log("WARNING: Child in \"" + gameObject.name + "\" with name other than up, down, left, or right.");
                    break;
            }
        }

        up.position = transform.position + camSize * Vector3.up;
        down.position = transform.position + camSize * Vector3.down;
        left.position = transform.position + camSize * aspect * Vector3.left;
        right.position = transform.position + camSize * aspect * Vector3.right;
        up.GetComponent<BoxCollider2D>().size = new Vector2(2*camSize*aspect,camSize*0.3f);
        down.GetComponent<BoxCollider2D>().size = new Vector2(2*camSize*aspect,camSize*0.3f);
        left.GetComponent<BoxCollider2D>().size = new Vector2(0.3f*camSize,camSize*2);
        right.GetComponent<BoxCollider2D>().size = new Vector2(0.3f*camSize,camSize*2);

        float mult = PlayerManager.count > 1 ? 1f : 0f;
        player1Spawn.position = Vector3.down * camSize * 0.4f + Vector3.left * camSize * mult;
        player2Spawn.position = Vector3.down * camSize * 0.4f + Vector3.right * camSize * 1f;
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
                _InitChildren();
                camSize = cam.orthographicSize;
            }
        }
        catch(Exception ex)
        {
            //Debug.Log(ex.Message);
        }

    }
}
