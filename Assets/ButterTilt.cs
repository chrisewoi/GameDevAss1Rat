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
        transform.up = transform.InverseTransformDirection(GetNormal().normalized); //transform.InverseTransformDirection(GroundCheck.GroundNormal());
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
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
