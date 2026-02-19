using JetBrains.Annotations;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Pool;
using UnityEngine.Splines;

public class EnemyScript : Poolable, IDamagable, IPoolable
{
    [SerializeField]
    private UnityEvent DeactivateEvent;
    [SerializeField]
    private UnityEvent DestroyedEvent;

    [SerializeField]
    private UnityEvent DamagedEvent;

    [SerializeField]
    private int health = 10;
    public int Health
    {
        get;
    }

    private bool activated = true;

    public bool Activated
    {
        get => activated;
        set
        {
            activated = value;
            if (!value)
            {
                DeactivateEvent?.Invoke();
            }
        }
    }


    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        SplineAnimate splAnim;
        if(TryGetComponent(out splAnim))
        {
            splAnim.Restart(false);
            splAnim.Container = null;
        }
    }

    protected new void Update()
    {
        if(!activated && !StaticFunctions.IsVisibleByCamera(rend))
        {
            ReleaseOrDestroy();
        }
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            GameObject.Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            DestroyedEvent?.Invoke();
            ReleaseOrDestroy();
        }
        else
        {
            DamagedEvent?.Invoke();
        }
    }

    public void SpawnExplosion(GameObject explosion)
    {
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
    }
}