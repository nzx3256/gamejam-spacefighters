using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10f)]
    public float moveSpeed = 5f;
    [Range(0.1f, 0.5f)]
    public float fireDelay = 0.3f;
    [SerializeField]
    private GameObject bulletPrefab;

    private Rigidbody2D rb;
    private Vector2 lastDirection = Vector2.zero;
    private bool FireButtonDown = false;
    private bool canFire = true;

    public void onMove(InputAction.CallbackContext ev)
    {
        lastDirection = ev.ReadValue<Vector2>();
    }

    public void onFire(InputAction.CallbackContext ev)
    {
        if(ev.started)
        {
             FireButtonDown = true;
        }
        else if(ev.canceled)
        {
             FireButtonDown = false;
        }
    }

    private void Start()
    {
        if(!TryGetComponent(out rb))
        {
            Debug.LogError("Rigidbody2D not set on " + gameObject.name);
        }
    }

    //Add to update if you want the code to run every frame
    private void Update()
    {
        if(FireButtonDown && canFire)
        {
            if(bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up*0.7f, Quaternion.identity);
                StartCoroutine(delayNextShot());
            }
        }
    }

    private void FixedUpdate()
    {
        //handle directional controls
        try
        {
            rb.velocity = Vector2.Lerp(rb.velocity, lastDirection * moveSpeed, 0.18f);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }


    }

    private IEnumerator delayNextShot(){
        canFire = false;
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    { 
        Debug.Log(other.collider.name);
    }
}
