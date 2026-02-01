using System.Collections;
using UnityEngine;

public class MoveWithCameraOnTimer : BaseMoveWithCamera
{
    [SerializeField]
    private float stopDelay = 5f;
    public override bool Move
    {
        get => move;
        set
        {
            if(move)
            {
                return;
            }
            if (value)
            {
                StartCoroutine(Timer());
            }
            base.Move = value;
        }
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(stopDelay);
        move = false;
    }
}
