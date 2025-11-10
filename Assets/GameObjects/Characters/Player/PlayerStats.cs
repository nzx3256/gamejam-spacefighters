using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int health = 10;
    public int Health
    {
        get => health;
    }

    public UnityEvent DestroyedEvent;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            DestroyedEvent?.Invoke();
            GameObject.Destroy(gameObject);
        }
    }
}
