using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class ShootSubroutine : MonoBehaviour
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

    private IEnumerator _refShootingSubroutine = null;
    public bool Shooting
    {
        get => _shooting;
        private set
        {
            if (value && _refShootingSubroutine == null)
            {
                _refShootingSubroutine = ShootingSubroutine();
                StartCoroutine(_refShootingSubroutine);
            }
            else if (!value && _refShootingSubroutine != null)
            {
                StopCoroutine(_refShootingSubroutine);
                _refShootingSubroutine = null;
            }
            _shooting = value;
        }
    }
    private IEnumerator ShootingSubroutine()
    {
        yield return new WaitForSeconds(shootingDelay);
        _shooting = true;
        foreach (var spawner in BulletShootSequence)
        {
            for (int i = 0; i < spawner.SpawnGroup.numToSpawn || spawner.SpawnGroup.spawnForever; i++)
            {
                spawner.fireAction?.Invoke();
                yield return new WaitForSeconds(spawner.SpawnGroup.delay);
            }
        }
        _shooting = false;
    }


    public void ShootOrNot(bool b)
    {
        Shooting = b;
    }

    // public void ToggleShooting()
    // {
    //     if(_refShootingSubroutine == null || !_shooting)
    //     {
    //         Shooting = true;
    //     }
    //     else if(_refShootingSubroutine != null || _shooting)
    //     {
    //         Shooting = false;
    //     }
    // }

    private void Start()
    {
        if (shootsBullets)
        {
            Shooting = true;
        }
    }
}