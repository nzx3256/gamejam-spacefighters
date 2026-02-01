using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserScript : SpecialAttackScript
{
    private enum LaserDirection
    {
        UP = 0,
        DOWN = 1
    }

    [SerializeField]
    private LaserDirection laserDirection;
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
    private Vector2 boxCastSize = new Vector2(0.5f, 0.5f);
    [SerializeField]
    private float castMaxDistance = 10f;

    [SerializeField] [Min(0f)]
    private float laserLengthSubractive = 0f;

    private string otherTag;
    private string otherBulletTag;

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
        if (tag.Contains("Player"))
        {
            otherTag = "Enemy";
            otherBulletTag = "EnemyBullet";
        }
        else if (tag.Contains("Enemy"))
        {
            otherTag = "Player";
            otherBulletTag = "PlayerBullet";
        }
    }

    private void Update()
    {
        if (activated)
        {
            bool found = false;
            float beamStopDistance = 0;
            Vector2 direction = DirectionIntToVector2(laserDirection);
            EnableVisuals(activated);
            LayerMask mask = ~LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxCastSize, 0, direction, castMaxDistance, mask);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag(otherTag))
                {
                    beamStopDistance = Mathf.Abs(hit.point.y - transform.position.y);
                    found = true;
                    if (canDamage)
                    {
                        hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
                        StartCoroutine(DamageWaitTime());
                    }
                    break;
                }
                if (hit.collider.CompareTag(otherBulletTag))
                {
                    GameObject.Destroy(hit.transform.gameObject);
                }
            }
            if (!found)
            {
                beamStopDistance = castMaxDistance;
            }
            midSpriteRenderer.size = new Vector2(midSpriteRenderer.size.x, beamStopDistance-laserLengthSubractive);
            LaserEnd.position = new Vector3(LaserMid.position.x, LaserMid.position.y + direction.y*(beamStopDistance-laserLengthSubractive), LaserMid.position.z);
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

    private Vector2 DirectionIntToVector2(LaserDirection dir)
    {
        switch (dir)
        {
            case LaserDirection.UP:
                return Vector2.up;
            case LaserDirection.DOWN:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    protected override void EnableVisuals(bool enable)
    {
        LaserStart.gameObject.SetActive(enable);
        LaserMid.gameObject.SetActive(enable);
        LaserEnd.gameObject.SetActive(enable);
    }
}
