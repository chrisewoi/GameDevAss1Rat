using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EdgeClimb : MonoBehaviour
{
    public bool canClimb;
    public float climbImpulse;
    public float climbDistance;
    //public LayerMask layerMask;

    private PlayerJumpMove player;
    public GameObject playerObject;
    private CapsuleCollider capsuleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = playerObject.GetComponent<PlayerJumpMove>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        canClimb = false;

        capsuleCollider.height = climbDistance + 1f;
        capsuleCollider.center = new Vector3(0, 0, climbDistance / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.coyoteJump) // Allow to climb again once player is able to jump again
        {
            canClimb = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if canClimb, player not moving backwards, and near edge in front of player
        if (canClimb && Input.GetAxis("Vertical") >= 0f && other.gameObject.CompareTag("Edge"))
        {
            print("Edge trigger " + other.name);
            // only continue if either holding jump or have been ungrounded for half a second
            //if (!Input.GetButton("Jump") || GroundCheck.UngroundedTime() < 0.5f) return;
            player.jumpValue += climbImpulse;
            float ungroundedTime = GroundCheck.UngroundedTime();
            if (ungroundedTime > player.jumpCharge) // Don't activate climb if it would slow player
            {
                player.jumpCharge = Mathf.Clamp01(ungroundedTime);
                canClimb = false;
            }
        }
    }


}
