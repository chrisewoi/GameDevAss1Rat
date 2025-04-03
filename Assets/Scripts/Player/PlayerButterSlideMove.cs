using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerButterSlideMove : MonoBehaviour, IMove
{
    private Vector3 velocity;
    private static bool onButter;
    public GameObject butterPrefab;
    private GameObject butter;
    public GameObject camera;
    public float butterYOffset;
    private PlayerJumpMove jumpScript;
    private float butterPickupY;
    public float mountTime;
    public float butterTiltTime;
    public float maxTiltAngle;
    public float timeActivated;
    public MeshCollider butterMeshCollider;
    private static float butterHeight;
    private bool runOnce;

    private Vector3 smoothNormal;
    MoveType IMove.moveType => MoveType.ButterSlide;
    Vector3 IMove.v => velocity;

    public float speed;

    private float yV, xV, zV;
    private Vector3 smoothNormalV;
    
    
    public float height;

    public float radius;

    public LayerMask layerMask;
    
    void Start()
    {
        jumpScript = GetComponent<PlayerJumpMove>();
        runOnce = true;
    }

    void Update()
    {
        if (onButter && butter != null)
        {
            if (runOnce)
            {
                runOnce = false;
                Destroy(butter.GetComponent<Rigidbody>());
                butterHeight = butterMeshCollider.bounds.size.y;
                Destroy(butterMeshCollider);
            }
            butter.transform.position = transform.position;
            butter.transform.position -= new Vector3(0,butterYOffset, 0);
            smoothNormal = Vector3.SmoothDamp(smoothNormal, GroundCheck.GroundNormal(), ref smoothNormalV, butterTiltTime);
            Vector3 angles = butter.transform.InverseTransformDirection(smoothNormal * maxTiltAngle);
            Vector3 currentAngles = transform.eulerAngles;
            //float smoothX = Mathf.SmoothDampAngle(currentAngles.x, angles.z, ref xV, butterTiltTime);
            //float smoothZ = Mathf.SmoothDampAngle(currentAngles.z, -angles.x, ref zV, butterTiltTime);
            
            angles.y = camera.transform.eulerAngles.y + 90f;
            float currentY = angles.y;  
            angles.y = Mathf.SmoothDampAngle(butter.transform.rotation.eulerAngles.y, angles.y, ref yV, 0.03f);
            //angles.z = GroundCheck.GroundNormal().z;
            //angles.z = butter.transform.InverseTransformDirection(GroundCheck.GroundNormal()).z;
            butter.transform.rotation = Quaternion.Euler(new Vector3(angles.z, angles.y, -angles.x));
            //butter.transform.rotation = Quaternion.Slerp(butter.transform.rotation, Quaternion.Euler(angles), Time.deltaTime * 100f);

            

            if (timeActivated > Time.time - mountTime)
            {
                Vector3 clampedPos = new Vector3(butter.transform.position.x, 
                                                 butterPickupY, 
                                                 butter.transform.position.z);
                butter.transform.position = clampedPos;
            }
        }
        else
        {
            butterHeight = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Butter"))
        {
            butterPickupY = other.transform.position.y;
            jumpScript.jumpValue = 1f; // force a jump
            onButter = true;
            butter = Instantiate(butterPrefab);
            Destroy(butter.GetComponent<Rigidbody>());
            butter.gameObject.tag = "Ground";
            butter.gameObject.transform.SetParent(transform);
            butterMeshCollider = butter.GetComponent<MeshCollider>();
            Destroy(other.gameObject);

            timeActivated = Time.time;
            Physics.SyncTransforms();
        }
    }

    public static bool IsSliding()
    {
        return onButter;
    }

    public static float ButterHeight()
    {
        return butterHeight;
    }
    
    Vector3 GetNormal()
    {
        if (GroundCheck.GroundNormal() == Vector3.zero) // No ground angle detected
        {
            return Vector3.up;
        }

        return GroundCheck.GroundNormal();
    }
}
