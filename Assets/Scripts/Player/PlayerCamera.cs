using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Vector2 angle;
    public static float yTilt;
    public static float yTiltMax;
    public float yTiltSmooth;
    public float smoothTime;

    public float setYTiltMax;

    public Vector2 yBounds;

    public static float mouseSensitivity;
    public float setMouseSensitivity;

    public SnootMove snootMove;

    public float v;
    
    void Start()
    {
        mouseSensitivity = setMouseSensitivity;
        yTiltMax = setYTiltMax;
        snootMove = GetComponentInChildren<SnootMove>();
        v = 0f;
    }

    void LateUpdate()
    {
        yTilt = snootMove.GetSmoothX();
        yTilt = math.clamp(yTilt, -yTiltMax, yTiltMax);
        yTiltSmooth = Mathf.SmoothDamp(yTiltSmooth, yTilt, ref v, smoothTime);

        var mouseDelta = Mouse.current.delta.ReadValue();
        angle += mouseDelta * (mouseSensitivity * Time.deltaTime);
        angle.y = math.clamp(angle.y, yBounds.x, yBounds.y);
        var xRot = Quaternion.AngleAxis(angle.x,Vector3.up);
        var yRot = Quaternion.AngleAxis(-angle.y +yTiltSmooth,Vector3.right);
        transform.localRotation= xRot*yRot;
    }
    public float GetYTilt()
    {
        return yTilt;
    }
}
