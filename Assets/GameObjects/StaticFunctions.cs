using UnityEngine;

public class StaticFunctions
{
    public static bool IsVisibleByCamera(SpriteRenderer spriteRenderer)
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
}
