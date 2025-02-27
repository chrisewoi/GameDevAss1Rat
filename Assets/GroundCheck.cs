using System;
using Unity.Mathematics;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static bool grounded;
    public float height;
    public float radius;
    public LayerMask layerMask;

    void Update()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        if (Physics.SphereCast(ray, radius, out var hitInfo, height / 2f, layerMask, QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);
            float angle = math.asin(math.dot(hitInfo.normal, Vector3.up))*math.TODEGREES;
            print($"normal={hitInfo.normal} hitPoint={hitInfo.point} colliderName={hitInfo.collider.gameObject.name} angle={angle}");
            grounded = true;
        }
        else
        {
            grounded = false;
        }

    }



    public static bool isGrounded()
    {
        return grounded;
    }
}
