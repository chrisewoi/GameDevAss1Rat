using UnityEngine;

public class RatShadowRotation : MonoBehaviour
{
    private Camera cam;

    private float rot => cam.transform.localEulerAngles.y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, rot + 180f, 0);
    }
}
