using System;
using UnityEngine;

public class ButterCollision : MonoBehaviour
{
    public Transform player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //gameObject.transform.SetParent(player);
        }
    }
}
