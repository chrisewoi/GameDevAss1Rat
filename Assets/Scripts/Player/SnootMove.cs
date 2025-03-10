using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SnootMove : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    
    public Vector2 angle;

    public float horizontalMove => Input.GetAxis("Horizontal");
    public float horizontalMoveSmooth;

    public Vector2 yBounds;

    public float mouseSensitivity => PlayerCamera.mouseSensitivity;

    public Vector3 targetXRot, rot;

    public float xRotMult, yRotMult, yMoveMult;

    public float xVel, yVel, moveVel, xSmoothTime, ySmoothTime;

    private float xSmoothTimeFalloff;

    public float smoothX, smoothY;

    public Camera cam;
    
    void Start()
    {
        //PlayerCamera.yTilt
        xVel = 0;
        yVel = 0;
        moveVel = 0;
        xSmoothTimeFalloff = 0;
    }

    void Update()
    {
        if (GroundCheck.isGrounded()) xSmoothTimeFalloff = xSmoothTime;
        xSmoothTimeFalloff -= Time.deltaTime;
        xSmoothTimeFalloff = math.clamp(xSmoothTimeFalloff, 0.15f, xSmoothTime);

        //zRot is banking which we never want
        //xRot is up/down tilt
        //yRot is turn left/right

        // project velocity onto the X plane to get target X rotation
        targetXRot = Vector3.ProjectOnPlane(playerMovement.GetVelocity(), transform.right).normalized;
        //print($"x tilt = {-targetXRot.y * xRotMult}. y tile = {targetXRot.x}");

        //float forwardBackward = Vector3.Dot(transform.forward, targetXRot);
        //bool movingBackwards = forwardBackward < 0;
        //float backwardsMult = movingBackwards ? -1 : 1;
        //print($"fb = {forwardBackward}. moveBackwards = {movingBackwards}");

        if (GroundCheck.isGrounded()) targetXRot.y = ((5 / xRotMult) + targetXRot.y ) /2f;

        // Smooth rotation
        smoothX = Mathf.SmoothDampAngle(transform.localEulerAngles.x, Mathf.Repeat(-targetXRot.y * xRotMult, 360f), ref xVel, xSmoothTimeFalloff);
        if (smoothX > 100f)
        {
            smoothX = -360 + smoothX;
        }
        //smoothX = math.clamp(smoothX, yBounds.x, yBounds.y);

        // CAMERA Y TILT
        //PlayerCamera.yTilt = smoothX;
        print("smoothX: " + smoothX);
        print("targetxrot.y = " + -targetXRot.y * xRotMult);
        print("targetXRot: " + targetXRot);

        var mouseDelta = Mouse.current.delta.ReadValue();
        angle += mouseDelta * mouseSensitivity * yRotMult;
        angle.x = math.clamp(angle.x, yBounds.x, yBounds.y);

        // both horizontalMove and mouseAngle values will be clamped to the yBounds and those bounds will be the max for both.
        horizontalMoveSmooth += horizontalMove * yBounds.y * yMoveMult * Time.deltaTime;
        horizontalMoveSmooth = math.clamp(horizontalMoveSmooth, yBounds.x, yBounds.y);
        horizontalMoveSmooth = Mathf.SmoothDamp(horizontalMoveSmooth, 0, ref moveVel, ySmoothTime);
        angle.x = Mathf.SmoothDamp(angle.x, 0, ref yVel, ySmoothTime);
        smoothY = angle.x + horizontalMoveSmooth /2f;
        //smoothY = Mathf.SmoothDamp(smoothY, 0, ref yVel, ySmoothTime * Time.deltaTime);
        

        // applying the x & y Rotation
        transform.localEulerAngles = new Vector3(smoothX, smoothY, transform.localEulerAngles.z);




        /*Vector3 final =
            Vector3.RotateTowards(transform.rotation.eulerAngles, new Vector3(0, targetRot.x, targetRot.y), 100f, 100f);
        transform.localEulerAngles = new Vector3(-vertVel * rotMult, horVel * rotMult);
        print(final);
        print("project: " + Vector3.ProjectOnPlane(playerMovement.GetVelocity() - PlayerGravityMove.GetGravityVelocity(), transform.right));
        */
    }
    public float GetSmoothX()
    {
        return smoothX;
    }
}
