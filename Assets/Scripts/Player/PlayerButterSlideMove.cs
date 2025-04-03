using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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
    public float timeActivated;
    public MeshCollider butterMeshCollider;
    private static float butterHeight;
    MoveType IMove.moveType => MoveType.ButterSlide;
    Vector3 IMove.v => velocity;

    public float speed;

    private float v;
    
    public float height;

    public float radius;

    public LayerMask layerMask;
    
    void Start()
    {
        jumpScript = GetComponent<PlayerJumpMove>();
    }

    void Update()
    {
        if (onButter && butter != null)
        {
            butter.transform.position = transform.position;
            butter.transform.position -= new Vector3(0,butterYOffset, 0);
            
            Vector3 angles = butter.transform.eulerAngles;
            
            angles.y = camera.transform.eulerAngles.y + 90f;
            float currentY = angles.y;
            angles.y = Mathf.SmoothDampAngle(butter.transform.rotation.eulerAngles.y, angles.y, ref v, 0.03f);
            angles.z = GroundCheck.GroundNormal().z;
            print(transform.InverseTransformDirection(GroundCheck.GroundNormal()));
            butter.transform.rotation = Quaternion.Euler(angles);
            //butter.transform.rotation = Quaternion.Slerp(butter.transform.rotation, Quaternion.Euler(angles), Time.deltaTime * 100f);

            butterHeight = butterMeshCollider.bounds.size.y;

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

    private void OnCollisionEnter(Collision other)
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
        float butterHeight = PlayerButterSlideMove.ButterHeight();
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        if (Physics.SphereCast(ray, radius + butterHeight, out var hitInfo, height / 2f, layerMask, QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);
            float angle = math.asin(math.dot(hitInfo.normal, Vector3.up))*math.TODEGREES;
            //print($"normal={hitInfo.normal} hitPoint={hitInfo.point} colliderName={hitInfo.collider.gameObject.name} angle={angle}");
            return new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z);

        }

        return Vector3.up;
    }
}
