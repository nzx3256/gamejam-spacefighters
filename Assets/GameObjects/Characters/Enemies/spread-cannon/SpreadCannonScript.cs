using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AimedShooting))]
public class SpreadCannonScript : MonoBehaviour
{
    [SerializeField]
    private Animator myAnim;

    [SerializeField]
    private Transform emitterTransform;

    private void Start()
    {
        if(myAnim == null && !TryGetComponent(out myAnim))
        {
            Debug.LogError("No Animator on "+gameObject.name+". Disabling " + this.name);
        }
        if(emitterTransform == null)
        {
            Debug.LogError("emitterTransform must be set to transform of an object");
        }
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while(gameObject.activeInHierarchy)
        {
            float angle = emitterTransform.rotation.eulerAngles.z;
            angle = angle > 180 ? angle - 360 : angle;
            myAnim.SetFloat("angle", angle);
            //Debug.Log(myAnim.GetFloat("angle"));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
