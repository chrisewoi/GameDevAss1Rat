using UnityEngine;

public class TrailOrientation : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
