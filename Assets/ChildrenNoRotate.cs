using UnityEngine;

public class ChildrenNoRotate : MonoBehaviour
{public Transform child;

    void Update ()
    {
        // stops child from having x rotation
        Vector3 childAngles = child.transform.rotation.eulerAngles;
        child.transform.rotation = Quaternion.Euler(new Vector3(0, childAngles.y, childAngles.z));
    }

}
