using UnityEngine;
using UnityEngine.Events;

public class EnemyScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private UnityEvent DeactivateEvent;
    [SerializeField]
    private UnityEvent DestroyedEvent;

    [SerializeField]
    private UnityEvent DamagedEvent;

    [SerializeField]
    private bool onDeactivationMoveOffScreen = true;


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

    private void Update()
    {
        ;
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
            GameObject.Destroy(gameObject);
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