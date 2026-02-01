using UnityEngine;

public class SpreadPelletContainerScript : MonoBehaviour, IStartVelocity
{
    [SerializeField]
    private Vector2 g_direction = Vector2.down;
    private float speedMultiplier = 1f;

    public Vector2 initialVelocity
    {
        get => g_direction;
        set {
            float angle = Vector2.SignedAngle(g_direction, value.normalized);
            speedMultiplier = value.magnitude;
            SetChildrenVelocities(angle);
        }
    }

    private void SetChildrenVelocities(float angle)
    {
        Matrix4x4 rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0,0,angle));
        foreach(Transform t in transform)
        {
            BulletScript script;
            if(t.TryGetComponent(out script))
            {
                script.initialVelocity = rotMatrix.MultiplyPoint3x4(script.initialVelocity*speedMultiplier);
            }
        }
    }
}
