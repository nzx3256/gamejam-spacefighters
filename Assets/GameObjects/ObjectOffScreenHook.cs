using UnityEngine;
using UnityEngine.Events;

public class ObjectOffScreenHook : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private UnityEvent offScreenEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null && cam.orthographic)
        {
            //Destroy the bullet if off screen
            float camSize = cam.orthographicSize;
            Vector2 camPosition = new Vector2(cam.transform.position.x, cam.transform.position.y); //Vector2 of camera position
            Vector2 ur = new Vector2(camSize * cam.aspect, camSize) + camPosition; // x is right, y is up
            Vector2 bl = (ur - camPosition) * -1 + camPosition; // x is left, y is down
            if (transform.position.x - 0.75f > ur.x || transform.position.x + 0.75f < bl.x ||
                transform.position.y - 0.75f > ur.y || transform.position.y + 0.75f < bl.y)
            {
                offScreenEvent?.Invoke();
            }
        }
        else if (Vector3.Distance(transform.position, cam.transform.position) > 10f)
        {
            offScreenEvent?.Invoke();
        }
    }

    public void DestroyThisObject()
    {
        GameObject.Destroy(gameObject);
    }
}
