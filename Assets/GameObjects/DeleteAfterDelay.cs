using System.Collections;
using UnityEngine;

public class DeleteAfterDelay : MonoBehaviour
{
    [SerializeField]
    private bool singleFrame;
    [SerializeField]
    private float delay = 1;
    private void Start()
    {
        StartCoroutine(deleteRoutine());
    }
    private IEnumerator deleteRoutine()
    {
        if (singleFrame)
        {
            yield return new WaitForEndOfFrame();
        }
        else {
            yield return new WaitForSeconds(delay);
        }
        GameObject.Destroy(gameObject);
    }

}
