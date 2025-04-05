using UnityEngine;

public class ButterMelt : MonoBehaviour
{
    public float butterSize01;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(butterSize01, butterSize01, butterSize01);
    }
}
