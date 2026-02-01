using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BulletScript))]
public class HomingMissileScript : MonoBehaviour
{
    private List<Transform> playerTransforms;
    private BulletScript bulletScript;

    [SerializeField]
    private float anglePerSecond = 10f;

    void Start()
    {
        playerTransforms = Array.ConvertAll<GameObject, Transform>(
            GameObject.FindGameObjectsWithTag("Player"), go => go.transform).ToList();
        bulletScript = GetComponent<BulletScript>();
        // bulletScript.initialVelocity = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPosition = EnemyMethods.ClosestPlayerPosition(ref playerTransforms, transform.position);
        Vector2 direction = playerPosition - (Vector2) transform.position;
        float targetAngle = Vector2.SignedAngle(bulletScript.initialVelocity, direction);
        float angle = anglePerSecond;
        if(targetAngle < 0f)
        {
            angle*=-1;
        }
        if(targetAngle == 0f)
        {
            return;
        }
        Matrix4x4 rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0,0,angle*Time.deltaTime));
        bulletScript.initialVelocity = rotMatrix.MultiplyPoint3x4(bulletScript.initialVelocity);
    }
}
