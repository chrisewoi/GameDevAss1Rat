using UnityEngine;

public class KeepLocalPosition : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics.SyncTransforms();
        rb.MovePosition(transform.position);
        rb.MoveRotation(transform.rotation);
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        //transform.localScale = Vector3.zero;
    }
}
