using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Vector2 angle;

    public Vector2 yBounds;

    public float mouseSensitivity;
    
    void Start()
    {
        
    }

    void Update()
    {
        var mouseDelta = Mouse.current.delta.ReadValue();
        angle += mouseDelta * (mouseSensitivity * Time.deltaTime);
        angle.y = math.clamp(angle.y, yBounds.x, yBounds.y);
        var xRot = Quaternion.AngleAxis(angle.x,Vector3.up);
        var yRot = Quaternion.AngleAxis(-angle.y,Vector3.right);
        transform.localRotation= xRot*yRot;
    }
}
