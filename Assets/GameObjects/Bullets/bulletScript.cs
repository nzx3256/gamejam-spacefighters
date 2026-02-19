using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : Poolable, IDamagable, IStartVelocity, IPoolable
{
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private float magnitude;
    public float Magnitude
    {
        get => magnitude;
    }
    public Vector2 initialVelocity
    {
        get { return direction.normalized*magnitude; }
        set { direction = value.normalized; magnitude = value.magnitude; }
    }
    private Collider2D col;
    private Rigidbody2D rb;

    [SerializeField]
    private int strength = 1;
    [SerializeField]
    private int health = 1;
    public int Health
    {
        get => health;
    }

    private Transform bulletContainer;

    [SerializeField]
    private bool indestructible = false;

    [SerializeField]
    private bool damagesOtherBullets;

    void Start()
    {
        GameObject go;
        if (bulletContainer != null || (go = GameObject.FindWithTag("BulletContainer")) != null && go.TryGetComponent(out bulletContainer))
        {
            transform.SetParent(bulletContainer);
        }
        else
        {
            transform.SetParent(null);
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (!TryGetComponent(out col))
        {
            Debug.LogError("No Collider on " + gameObject.name + ". Destroying Object");
            Destroy(gameObject);
        }
        else
        {
            col.isTrigger = true;
        }
    }

    protected new void Update()
    {
        base.Update();
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
            ReleaseOrDestroy();
        }
    }
}