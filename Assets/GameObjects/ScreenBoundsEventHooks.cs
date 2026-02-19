using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenBoundsEventHooks : MonoBehaviour
{

    public UnityEvent offScreenEvent;
    public UnityEvent onScreenEvent;

    private Camera cam;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool b = IsVisibleByCamera();
        if(b)
        {
            onScreenEvent?.Invoke();
        }
        else 
        {
            offScreenEvent?.Invoke();
        }

    } 

    private bool IsVisibleByCamera()
    {
        if (spriteRenderer == null)
            return false;

        Camera[] cams = new Camera[Camera.allCamerasCount];
        Camera.GetAllCameras(cams);
        foreach (var c in cams)
        {
            if (c == null)
                continue;
            // Ignore Scene view / editor cameras
            if (c.cameraType == CameraType.SceneView || c.name == "SceneCamera")
                continue;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
            if (GeometryUtility.TestPlanesAABB(planes, spriteRenderer.bounds))
                return true;
        }
        return false;
    }

    public void DestroyThisObject()
    {
        GameObject.Destroy(gameObject);
    }
}
