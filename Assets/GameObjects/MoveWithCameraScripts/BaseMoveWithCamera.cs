using UnityEngine;

public class BaseMoveWithCamera : MonoBehaviour
{
    [SerializeField]
    private bool moveOnAwake = false;
    protected Vector3 offsetToCam = Vector3.zero;
    private bool hasRigidbody = false;
    private Rigidbody2D rb;
    protected bool move = false;

    private Vector2 lastVelocity = Vector2.zero;

    private MoveCamera moveCameraComponent;

    public virtual bool Move
    {
        get => move;
        set
        {
            move = value;
            if(value && !hasRigidbody)
            {
                offsetToCam = (transform.position - Camera.main.transform.position);
            }
        }
    }

    private void Awake()
    {
        if (TryGetComponent(out rb))
        {
            hasRigidbody = true;
        }
        if (moveOnAwake)
        {
            Move = true;
        }
    }

    private void Start()
    {
        if(!Camera.main.TryGetComponent(out moveCameraComponent))
        {
            Debug.Log("WARNING: Camera does not have a MoveCamera Component.");
            this.enabled = false;
        }
    } 

    protected void LateUpdate()
    {
        if(hasRigidbody && move)
        {
            if(lastVelocity != rb.linearVelocity)
            {
                rb.linearVelocityY += moveCameraComponent.upwardVelocity;
                lastVelocity = rb.linearVelocity;
            }
        }
        else if(move)
        {
            transform.position = Camera.main.transform.position + offsetToCam;
        }
    }
}