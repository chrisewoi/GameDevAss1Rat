using UnityEngine;

public class ButterTilt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (!TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.InverseTransformDirection(GroundCheck.GroundNormal());
    }
}
