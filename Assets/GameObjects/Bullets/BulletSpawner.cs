using UnityEngine;
enum ShootType
    {
        BIDIRECTIONAL = 0,
        CIRCULAR,
        STRAIGHT,
    }
public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    private ShootType shootType = ShootType.STRAIGHT;
    [SerializeField]
    [Tooltip("Distance between each bullet")]
    [Range(0.1f, Mathf.PI * 2)]
    private float stride = 1f;
    [SerializeField]
    [Tooltip("Number of bullets to spawn at once")]
    private uint spawnWidth = 1;
    [SerializeField]
    private float distance = 0f;
    private float sizeScalar = 0.5f;
    [SerializeField]
    private float bulletSpeed = 1f;
    [SerializeField]
    private GameObject bulletPrefab;

    public uint SpawnWidth
    {
        get => spawnWidth;
        set => spawnWidth = value;
    }
    public float Stride
    {
        get => stride;
        set => stride = value;
    }

    private void OnDrawGizmosSelected()
    {
        float totalWidth;
        if (bulletPrefab != null) Gizmos.color = Color.cyan;
        else Gizmos.color = Color.yellow;
        switch (shootType)
        {
            case ShootType.BIDIRECTIONAL:
                Vector3 cl = transform.position + transform.right * -1 * distance;
                Vector3 cr = transform.position + transform.right * distance;
                Gizmos.DrawLineStrip(
                new Vector3[3] {
                    transform.right*-0.5f*sizeScalar + cl,
                    (transform.right*0 + transform.up*0.5f)*sizeScalar + cl,
                    (transform.right*0 + transform.up*-0.5f)*sizeScalar + cl
                }, true);
                Gizmos.DrawLineStrip(
                new Vector3[3] {
                    transform.right*0.5f*sizeScalar + cr,
                    (transform.right*0 + transform.up*0.5f)*sizeScalar + cr,
                    (transform.right*0 + transform.up*-0.5f)*sizeScalar + cr
                }, true);
                totalWidth = (spawnWidth - 1) * stride;
                for (int i = 0; i < spawnWidth; i++)
                {
                    DrawPlus(totalWidth / 2 * transform.up + i * transform.up * stride * -1 + cl);
                    DrawPlus(totalWidth / 2 * transform.up + i * transform.up * stride * -1 + cr);
                }
                break;
            case ShootType.CIRCULAR:
                bool closed = stride >= Mathf.PI * 2 - 0.1f ? true : false;
                Vector3[] points = new Vector3[spawnWidth];
                float delta = stride / (spawnWidth);
                for (int i = 0; i < spawnWidth; i++)
                {
                    float theta = Mathf.PI * 1.5f - stride / 2 + delta / 2 + i * delta + transform.eulerAngles.z * Mathf.PI / 180;
                    points[i].x = sizeScalar * distance * Mathf.Cos(theta) + transform.position.x;
                    points[i].y = sizeScalar * distance * Mathf.Sin(theta) + transform.position.y;
                    points[i].z = transform.position.z;
                }
                Gizmos.DrawLineStrip(points, closed);
                break;
            case ShootType.STRAIGHT:
                Vector3 c = transform.position + transform.up * -1 * distance;
                Gizmos.DrawLineStrip(
                new Vector3[3] {
                    transform.up*-0.7f*sizeScalar + c,
                    (transform.right*0.5f + transform.up*0.5f)*sizeScalar + c,
                    (transform.right*-0.5f + transform.up*0.5f)*sizeScalar + c
                }, true);
                totalWidth = stride * (spawnWidth - 1);
                for (int i = 0; i < spawnWidth; i++)
                {
                    DrawPlus(totalWidth / 2f * transform.right * -1 + transform.right * stride * i + transform.position);
                }
                break;
        }

    }

    private void DrawPlus(Vector3 pos, float size = 0.2f)
    {
        Gizmos.DrawLine(new Vector3(-1 * size, 0) * sizeScalar + pos, new Vector3(size, 0) * sizeScalar + pos);
        Gizmos.DrawLine(new Vector3(0, -1 * size) * sizeScalar + pos, new Vector3(0, size) * sizeScalar + pos);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }


    public void Spawn()
    {
        if (bulletPrefab == null)
        {
            return;
        }
        float totalWidth;
        switch (shootType)
        {
            case ShootType.BIDIRECTIONAL:
                Vector3 cl = transform.position + transform.right * -1 * distance;
                Vector3 cr = transform.position + transform.right * distance;
                totalWidth = (spawnWidth - 1) * stride;
                for (int i = 0; i < spawnWidth; i++)
                {
                    Vector3 point = totalWidth / 2 * transform.up + i * transform.up * stride * -1 + cl;
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, point, Quaternion.identity);
                    bullet.GetComponent<BulletScript>().initialVelocity = bulletSpeed * transform.right * -1;
                    point += cr - cl;
                    bullet = GameObject.Instantiate(bulletPrefab, point, Quaternion.identity);
                    bullet.GetComponent<BulletScript>().initialVelocity = bulletSpeed * transform.right;
                }
                break;
            case ShootType.CIRCULAR:
                //Vector2[] points = new Vector2[spawnWidth];
                float delta = stride / (spawnWidth);
                for (int i = 0; i < spawnWidth; i++)
                {
                    float theta = Mathf.PI * 1.5f - stride / 2 + delta / 2 + i * delta + transform.eulerAngles.z * Mathf.PI / 180;
                    float x = sizeScalar * distance * Mathf.Cos(theta) + transform.position.x;
                    float y = sizeScalar * distance * Mathf.Sin(theta) + transform.position.y;
                    float z = transform.position.z;
                    Vector2 direction = (new Vector2(x, y) - (Vector2)transform.position).normalized;
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector3(x, y, z), Quaternion.identity);
                    bullet.GetComponent<BulletScript>().initialVelocity = bulletSpeed * direction;
                }
                break;
            case ShootType.STRAIGHT:
                totalWidth = stride * (spawnWidth - 1);
                for (int i = 0; i < spawnWidth; i++)
                {
                    Vector3 point = totalWidth / 2f * transform.right * -1 + transform.right * stride * i + transform.position;
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, point, Quaternion.identity);
                    bullet.GetComponent<BulletScript>().initialVelocity = bulletSpeed * transform.up * -1;
                }
                break;
            default:
                break;
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetRotation(float zRotation)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * zRotation);
        transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z%360);
    }

    public void RotateTransform(float zRotation)
    {
        transform.Rotate(transform.forward, zRotation);
        transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z%360);
    }
}
