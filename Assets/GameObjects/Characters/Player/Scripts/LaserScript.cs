using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LaserScript : SpecialAttackScript
{
    [SerializeField]
    private Transform LaserStart;
    [SerializeField]
    private Transform LaserMid;
    [SerializeField]
    private Transform LaserEnd;

    [SerializeField]
    private SpriteRenderer midSpriteRenderer;

    [SerializeField]
    private int damage = 4;
    [SerializeField] [Range(0.1f, 5f)]
    private float cooldown = 0.2f;

    private bool canDamage = true;

    [SerializeField]
    private UnityEngine.Vector2 boxCastSize = new UnityEngine.Vector2(0.5f, 0.5f);
    [SerializeField]
    private float castMaxDistance = 10f;

    private void Start()
    {
        if (midSpriteRenderer == null)
        {
            Debug.Log("midSpriteRenderer is unassigned. Disabling this script.");
            this.enabled = false;
        }
        if (LaserEnd == null || LaserMid == null || LaserStart == null)
        {
            Debug.Log("LaserStart, LaserMid, and LaserEnd must be set in LaserScript. Disabling this Script");
        }
    }

    private void Update()
    {
        if (activated)
        {
            bool found = false;
            float beamStopDistance = 0;
            EnableVisuals(activated);
            LayerMask mask = LayerMask.GetMask("Player");
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxCastSize, 0, Vector2.up, castMaxDistance);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    beamStopDistance = hit.point.y - transform.position.y;
                    found = true;
                    if (canDamage)
                    {
                        hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
                        StartCoroutine(DamageWaitTime());
                    }
                    break;
                }
                if (hit.collider.CompareTag("EnemyBullet"))
                {
                    GameObject.Destroy(hit.transform.gameObject);
                }
            }
            if (!found)
            {
                beamStopDistance = castMaxDistance;
            }
            midSpriteRenderer.size = new Vector2(midSpriteRenderer.size.x, beamStopDistance);
            LaserEnd.position = new Vector3(LaserMid.position.x, LaserMid.position.y + beamStopDistance, LaserMid.position.z);
        }
        else
        {
            EnableVisuals(activated);
        }
    }

    private IEnumerator DamageWaitTime()
    {
        canDamage = false;
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }

    protected override void EnableVisuals(bool enable)
    {
        LaserStart.gameObject.SetActive(enable);
        LaserMid.gameObject.SetActive(enable);
        LaserEnd.gameObject.SetActive(enable);
    }
}
