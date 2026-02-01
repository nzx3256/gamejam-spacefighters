using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenBoundsEventHooks : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float padding = 0.75f;

    public UnityEvent offScreenEvent;
    public UnityEvent onScreenEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        StartCoroutine(EventCheckCoroutine());
    }

    private IEnumerator EventCheckCoroutine()
    {
        while (true)
        {
            try {
                if (cam != null && cam.orthographic)
                {
                    //Destroy the bullet if off screen
                    float camSize = cam.orthographicSize;
                    Vector2 camPosition = new Vector2(cam.transform.position.x, cam.transform.position.y); //Vector2 of camera position
                    Vector2 ur = new Vector2(camSize * cam.aspect, camSize) + camPosition; // x is right, y is up
                    Vector2 bl = (ur - camPosition) * -1 + camPosition; // x is left, y is down
                    if (transform.position.x - padding > ur.x || transform.position.x + padding < bl.x ||
                        transform.position.y - padding > ur.y || transform.position.y + padding < bl.y)
                    {
                        offScreenEvent?.Invoke();
                    }
                    else
                    {
                        onScreenEvent?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void DestroyThisObject()
    {
        GameObject.Destroy(gameObject);
    }
    
    public void log(string s)
    {
        Debug.Log(s+" | "+transform.position);
    }
}
