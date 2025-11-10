using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 10f)]
    public float moveSpeed = 5f;
    [Range(0.1f, 0.5f)]
    public float fireDelay = 0.3f;
    [SerializeField]
    private float bulletSpawnYoffset = 0f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private bool moveWithCamera = true;
    [SerializeField]
    private SpecialAttackScript specialAttack;
    [SerializeField]
    private float specialCooldownTime = 30f;
    [Range(0f,30f)] [SerializeField]
    private float specialDuration = 10f;
    [SerializeField]
    private Animator anim;

    private Rigidbody2D rb;
    private Vector2 lastDirection = Vector2.zero;
    private bool FireButtonDown = false;
    private bool canFire = true;
    private bool SpecialButtonDown = false;
    private bool isSpecialAvailable = true;
    private Vector2 deltaPosition = Vector2.zero;

    public void onMove(InputAction.CallbackContext ev)
    {
        lastDirection = ev.ReadValue<Vector2>().normalized;
    }

    public void onFire(InputAction.CallbackContext ev)
    {
        if (ev.started)
        {
            FireButtonDown = true;
        }
        else if (ev.canceled)
        {
            FireButtonDown = false;
        }
    }

    public void onSpecial(InputAction.CallbackContext ev)
    {
        if (ev.started)
        {
            SpecialButtonDown = true;
        }
        else if (ev.canceled)
        {
            SpecialButtonDown = false;
        }
        Debug.Log(SpecialButtonDown);
    }

    private void Start()
    {
        if (!TryGetComponent(out rb))
        {
            Debug.LogError("Rigidbody2D not set on " + gameObject.name);
        }
        if (anim == null && !TryGetComponent(out anim))
        {
            Debug.LogError("No animator on " + gameObject.name);
        }
    }

    //Add to update if you want the code to run every frame
    private void Update()
    {
        if (FireButtonDown && canFire)
        {
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + Vector3.up * bulletSpawnYoffset, Quaternion.identity);
                bullet.GetComponent<BulletScript>().initialVelocity = Vector2.up * (8f + rb.linearVelocityY);
                StartCoroutine(delayNextShot());
            }
        }
        if (SpecialButtonDown && isSpecialAvailable)
        {
            if (specialAttack != null)
            {
                StartCoroutine(activateSpecialAttack());
            }
        }
        //rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, lastDirection.normalized * moveSpeed, 0.18f);
        rb.linearVelocity = lastDirection * moveSpeed;
        anim.SetFloat("xdir", lastDirection.x);
    }

    private IEnumerator delayNextShot()
    {
        canFire = false;
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    private IEnumerator activateSpecialAttack()
    {
        specialAttack.activated = true;
        yield return new WaitForSeconds(specialDuration);
        specialAttack.activated = false;
        StartCoroutine(specialCooldown(specialCooldownTime));
    }

    private IEnumerator specialCooldown(float timing)
    {
        isSpecialAvailable = false;
        yield return new WaitForSeconds(timing);
        isSpecialAvailable = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
