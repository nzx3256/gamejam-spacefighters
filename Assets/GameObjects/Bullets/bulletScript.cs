using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour, IDamagable
{
    public Vector2 initialVelocity;
    private Collider2D col;
    private Rigidbody2D rb;
    private Camera cam;

    [SerializeField]
    private int strength = 1;
    [SerializeField]
    private int health = 1;
    public int Health
    {
        get => health;
    }

    [SerializeField]
    private bool indestructible = false;

    [SerializeField]
    private bool damagesOtherBullets;

    void Start()
    {
        Transform bulletContainer = GameObject.FindWithTag("BulletContainer").transform;
        if (bulletContainer != null)
        {
            transform.SetParent(bulletContainer);
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (!TryGetComponent(out col))
        {
            Debug.LogError("No Collider on " + gameObject.name + ". Destroying Object");
            Destroy(gameObject);
        }
        col.isTrigger = true;
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Could not find main camera. Destroying " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (rb.linearVelocity != initialVelocity)
        {
            rb.linearVelocity = initialVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("PlayerBullet"))
        {
            return;
        }
        else if (other.CompareTag("Enemy") && gameObject.CompareTag("EnemyBullet"))
        {
            return;
        }
        if (other.CompareTag("PlayerBullet") && gameObject.CompareTag("PlayerBullet"))
        {
            return;
        }
        if (other.CompareTag("EnemyBullet") && gameObject.CompareTag("EnemyBullet"))
        {
            return;
        }
        IDamagable script;
        if (other.TryGetComponent(out script))
        {
            if(script.GetType() == typeof(BulletScript) && !damagesOtherBullets)
            {
                ;
            }
            else
            {
                script.TakeDamage(strength);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if (dmg < 0) return;
        health -= dmg;
        if (health <= 0 && !indestructible)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
