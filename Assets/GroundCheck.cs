using System;
using Unity.Mathematics;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool grounded;
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
            float angle = math.asin(math.dot(hitInfo.normal, Vector3.up))*math.TODEGREES;
            print($"normal={hitInfo.normal} hitPoint={hitInfo.point} colliderName={hitInfo.collider.gameObject.name} angle={angle}");
        }
    }



    public bool isGrounded()
    {
        return grounded;
    }
}
