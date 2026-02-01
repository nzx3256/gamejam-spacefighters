using UnityEngine;

public class MoveToCameraOffset : MonoBehaviour
{
    public Vector2 offset = Vector2.zero;
    public void moveIntoScreen()
    {
        Vector2 center = Camera.main.transform.position;
        transform.position = center + offset;
    }
}
