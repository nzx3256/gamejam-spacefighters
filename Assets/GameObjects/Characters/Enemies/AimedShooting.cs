using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AimedShooting : MonoBehaviour
{
    [SerializeField]
    private UnityEvent targetAngleReached;
    [SerializeField]
    private float fireDelay = 0.5f;
    [SerializeField]
    private GameObject emitter;
    [SerializeField]
    private bool eightDirection = false;
    [SerializeField]
    private bool forwardShootingOnly = true;
    [SerializeField]
    private float paddingAngle = 35.0f;
    
    [SerializeField]
    private List<Transform> playerTransforms;
    private int targetIndex;

    private bool canfire = true;
    public bool CanFire
    {
        get => canfire;
    }

    private void Start()
    {
        if(emitter == null)
        {
            Debug.LogError("ERROR! Emitter must be set in inspector for "+this.name+" Components.\nDisabling Script");
        }
        playerTransforms = System.Array.ConvertAll<GameObject,Transform>(
            GameObject.FindGameObjectsWithTag("Player"), go => go.transform).ToList();
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = EnemyMethods.ClosestPlayerPosition(ref playerTransforms, transform.position);
        float targetAngle = Vector2.SignedAngle(Vector2.down, playerPosition - (Vector2)transform.position);
        if(canfire && (!forwardShootingOnly || targetAngle < 90 + paddingAngle && targetAngle > -90 - paddingAngle))
        {
            if (eightDirection)
            {
                targetAngle = Mathf.Round(targetAngle/45);
                targetAngle*=45;
            }
            float angle = Mathf.LerpAngle(emitter.transform.rotation.eulerAngles.z, targetAngle, 0.1f);
            angle = angle > 180 ? angle - 360 : angle;
            if(forwardShootingOnly)
            {
                angle = Mathf.Clamp(angle, -90f, 90f);
            }
            emitter.transform.rotation = Quaternion.Euler(0,0,angle);
            if(Mathf.Abs(angle - targetAngle) < 3f)
            {
                targetAngleReached?.Invoke();
                StartCoroutine(fireDelayCoroutine());
            }
        }
    }

    private IEnumerator fireDelayCoroutine()
    {
        canfire = false;
        yield return new WaitForSeconds(fireDelay);
        canfire = true;
    }
}
