using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SnootMove : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    
    public Vector2 angle;

    public Vector2 yBounds;

    public float mouseSensitivity;

    public Vector3 targetRot, rot;

    public float rotMult;

    public Camera cam;
    
    void Start()
    {
        
    }

    void Update()
    {
        targetRot = playerMovement.GetVelocity().normalized;
        float horizontalMagnitude = targetRot.x * targetRot.z; //* math.dot(transform.forward, Vector3.forward) * math.dot(transform.right, Vector3.right) * math.dot(transform.up, Vector3.up);
        float vertVel, horVel;
        horVel = horizontalMagnitude;
        vertVel = targetRot.y;
        
        float xTarget = targetRot.x * targetRot.z;
        var mouseDelta = Mouse.current.delta.ReadValue();
        angle += mouseDelta * (mouseSensitivity * Time.deltaTime);
        angle.y = math.clamp(angle.y, yBounds.x, yBounds.y);
        //var xRot = Quaternion.AngleAxis(angle.x,Vector3.up);
        //var yRot = Quaternion.AngleAxis(-angle.y,Vector3.right);
        
        //var xRot = Quaternion.Euler(xTarget, 0 ,0);
        
        //rot = Vector3.RotateTowards(transform.rotation.eulerAngles, horizontalMagnitude * transform.forward + vertVel * Vector3.up, 100f, 100f);
        Vector3 final =
            Vector3.RotateTowards(transform.rotation.eulerAngles, new Vector3(0, vertVel, horVel), 100f, 100f);
        //transform.rotation.eulerAngles.Set(0, final.y, final.z);
        transform.localEulerAngles = new Vector3(-vertVel * rotMult, horVel * rotMult);
        /*if (GroundCheck.isGrounded())
        {
            transform.up = GroundCheck.GroundNormal();
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, horVel * rotMult);
        }*/
        //if(GroundCheck.isGrounded()) transform.localEulerAngles = 
        ;// * rotMult;
        //transform.rotation.SetLookRotation(final);
        print(final);
            //Quaternion.Euler();
        //transform.Rotate(xTarget, 0, 0);
        //transform.rotation = Quaternion.Euler(rot);
    }
}
