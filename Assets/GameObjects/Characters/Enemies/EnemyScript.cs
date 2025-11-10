using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;

public class EnemyScript : MonoBehaviour, IDamagable
{
    [Tooltip("Whether the enemy will start shooting or not after spawning.")]
    [SerializeField]
    private bool shootsBullets = false;
    [SerializeField]
    private List<BulletSpawnGroupWrapper> BulletShootSequence;
    [SerializeField]
    [Tooltip("Delay in seconds until enemy starts shooting.\n" +
        "Does nothing if \"Shoots Bullets\" field is false.")]
    private float shootingDelay = 2f;

    private bool _shooting = false;

    [SerializeField]
    private UnityEvent DeactivateEvent;
    [SerializeField]
    private UnityEvent DestroyedEvent;

    [SerializeField]
    private UnityEvent DamagedEvent;

    public bool shooting
    {
        get => _shooting;
    }

    [SerializeField]
    private int health = 10;
    public int Health
    {
        get;
    }

    private IEnumerator ShootingSubroutine()
    {
        yield return new WaitForSeconds(shootingDelay);
        _shooting = true;
        foreach (var spawner in BulletShootSequence)
        {
            for (int i = 0; i < spawner.SpawnGroup.numToSpawn; i++)
            {
                spawner.fireAction?.Invoke();
                yield return new WaitForSeconds(spawner.SpawnGroup.delay);
            }
        }
        _shooting = false;
    }

    private void Start()
    {
        if (shootsBullets)
        {
            StartCoroutine(ShootingSubroutine());
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
