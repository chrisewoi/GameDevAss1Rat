using System;
using UnityEngine;

public class PlayerButterSlideMove : MonoBehaviour, IMove
{
    private Vector3 velocity;
    public bool onButter;
    public GameObject butterPrefab;
    private GameObject butter;
    public PlayerJumpMove jumpScript;
    MoveType IMove.moveType => MoveType.ButterSlide;
    Vector3 IMove.v => velocity;

    public float speed;
    public bool sliding;
    
    void Start()
    {
        jumpScript = GetComponent<PlayerJumpMove>();
    }

    void Update()
    {
        if (onButter)
        {
            butter.transform.position = transform.position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Butter"))
        {
            jumpScript.jumpValue = 1f; // force a jump
            onButter = true;
            butter = Instantiate(butterPrefab);
            butter.gameObject.transform.SetParent(transform);
            Destroy(other.gameObject);
        }
    }
}
