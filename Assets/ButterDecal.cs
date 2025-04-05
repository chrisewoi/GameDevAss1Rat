using UnityEngine;

public class ButterDecal : MonoBehaviour
{
    public GameObject decalPrefab;
    public Transform decalParent;
    public float timeBetweenDecals = 0.2f;
    public float decalLifetime;
    private float timer;
    
    void Start()
    {
        decalParent = ButterTrailDecalsParent.butterTrailDecalsParent;
    }

    void Update()
    {
        if (timer >= timeBetweenDecals)
        {
            timer = 0f;
            GameObject decal = Instantiate(decalPrefab, transform.position, transform.rotation, decalParent);
            Destroy(decal.GetComponent<ButterDecal>());
            Destroy(decal, decalLifetime);
        }
        
        timer += Time.deltaTime;
    }
}
