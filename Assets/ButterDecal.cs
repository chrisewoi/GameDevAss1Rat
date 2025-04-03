using UnityEngine;

public class ButterDecal : MonoBehaviour
{
    public GameObject decalPrefab;
    public float timeBetweenDecals = 0.2f;
    public float decalLifetime;
    private float timer;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (timer >= timeBetweenDecals)
        {
            timer = 0f;
            GameObject decal = Instantiate(decalPrefab, transform.position, transform.rotation);
            Destroy(decal.GetComponent<ButterDecal>());
            Destroy(decal, decalLifetime);
        }
        
        timer += Time.deltaTime;
    }
}
