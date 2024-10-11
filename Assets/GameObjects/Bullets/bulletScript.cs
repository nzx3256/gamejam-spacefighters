using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class bulletScript : MonoBehaviour
{
    public Vector2 velocity2;
    private Collider2D col;
    private Rigidbody2D rb;
    private Camera cam;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if(!TryGetComponent(out col))
        {
            Debug.LogError("No Collider on " + gameObject.name+ ". Destroying Object");
            Destroy(gameObject);
        }
        col.isTrigger = true;
        cam = Camera.main;
        if(cam == null)
        {
            Debug.LogError("Could not find main camera. Destroying " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(rb.velocity != velocity2)
        {
            rb.velocity = velocity2;
        }
        if(cam != null && cam.orthographic){
            //Destroy the bullet if off screen
            float camSize = cam.orthographicSize;
            Vector2 camPosition = new Vector2(cam.transform.position.x, cam.transform.position.y); //Vector2 of camera position
            Vector2 ur = new Vector2(camSize*cam.aspect,camSize) + camPosition; // x is right, y is up
            Vector2 bl = ur*-1 + camPosition; // x is left, y is down
            if(transform.position.x - 0.5f > ur.x || transform.position.x + 0.5f < bl.x || transform.position.y - 0.5f > ur.y || transform.position.y + 0.5f < bl.y) 
            {
                Destroy(gameObject);
            }
        }
        else if(Vector3.Distance(transform.position, cam.transform.position) > 10f) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("bullet"))
        {
            //Damage other bullets

        }
    }
}
