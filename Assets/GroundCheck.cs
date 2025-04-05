using System;
using Unity.Mathematics;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static bool grounded;
    public float height;
    public float radius;
    public LayerMask layerMask;
    private static float ungroundedTime = 0f;
    private static float groundedTime = 0f;
    public float airborneTime;

    public bool isSliding => PlayerButterSlideMove.IsSliding();

    private bool printChange;
    private int count;
    private static Vector3 groundAngle;

    void Update()
    {
        float butterHeight = PlayerButterSlideMove.ButterHeight();
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        if (Physics.SphereCast(ray, radius + butterHeight, out var hitInfo, height / 2f, layerMask, QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);
            float angle = math.asin(math.dot(hitInfo.normal, Vector3.up))*math.TODEGREES;
            //print($"normal={hitInfo.normal} hitPoint={hitInfo.point} colliderName={hitInfo.collider.gameObject.name} angle={angle}");
            groundAngle = new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z);
            grounded = true;
            if(ungroundedTime > 0f) airborneTime = ungroundedTime;
            ungroundedTime = 0f;
            groundedTime += Time.deltaTime;
        }
        else
        {
            grounded = false;
            groundAngle = Vector3.zero;
            ungroundedTime += Time.deltaTime;
            groundedTime = 0f;
        }

        // print if I'm grounded only when value changes
        if (printChange == grounded)
        {
            print("Grounded (" + count + "): " + grounded + ", Time Airborne: " + airborneTime);
            printChange = !printChange;
            count++;
        }
    }



    public static bool isGrounded()
    {
        return grounded;
    }

    public static float UngroundedTime()
    {
        return ungroundedTime;
    }

    public static float GroundedTime()
    {
        return groundedTime;
    }

    public static Vector3 GroundNormal()
    {
        return groundAngle;
    }
}
