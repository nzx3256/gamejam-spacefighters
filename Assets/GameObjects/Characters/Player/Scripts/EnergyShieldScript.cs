using System;
using UnityEngine;

public class EnergyShieldScript : SpecialAttackScript
{
    [SerializeField]
    private Transform ShieldVisual;

    [SerializeField]
    private float radius = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ShieldVisual == null)
        {
            Debug.Log("Must set SheildVisual to child of this object \"" + gameObject.name + "\"");
        }
    }

    private void OnDrawGizmosSelected()
    {
        int numpoints = 20;
        bool closed = true;
        Vector3[] points = new Vector3[numpoints];
        for(int i = 0; i < numpoints; i++)
        {
            points[i].x = (float)Math.Cos(Math.PI*2/numpoints*i)*radius;
            points[i].y = (float)Math.Sin(Math.PI*2/numpoints*i)*radius;
            points[i].z = 0;
        }
        Gizmos.DrawLineStrip(points,closed);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            EnableVisuals(activated);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,radius,Vector2.zero);
            foreach(var hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("EnemyBullet"))
                {
                    GameObject.Destroy(hit.collider.gameObject);
                }
            }
        }
        else
        {
            EnableVisuals(activated);
        }
    }

    protected override void EnableVisuals(bool enable)
    {
        ShieldVisual.gameObject.SetActive(enable);
    } 
}
