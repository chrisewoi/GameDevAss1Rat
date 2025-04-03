using Unity.Mathematics;
using UnityEngine;

public class ButterTilt : MonoBehaviour
{
    public Vector3 target;

    public float tiltSpeed;

    public float height;

    public float radius;

    public LayerMask layerMask;

    private MeshCollider butterMeshCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (!TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            butterMeshCollider = GetComponent<MeshCollider>();
            radius = butterMeshCollider.bounds.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float yRot = transform.rotation.eulerAngles.y;
        transform.up = GetNormal(); //transform.InverseTransformDirection(GroundCheck.GroundNormal());
        print("butter normal: " + transform.up);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
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
