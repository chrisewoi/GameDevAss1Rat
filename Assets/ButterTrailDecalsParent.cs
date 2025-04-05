using System;
using UnityEngine;

public class ButterTrailDecalsParent : MonoBehaviour
{
    public static Transform butterTrailDecalsParent;
    private new static string name;

    void Start()
    {
        butterTrailDecalsParent = transform;
        name = gameObject.name + " (Count: ";
    }

    private void FixedUpdate()
    {
        gameObject.name = name + transform.childCount + ")";
    }
}
