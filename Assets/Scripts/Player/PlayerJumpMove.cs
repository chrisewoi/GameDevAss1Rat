using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpMove : MonoBehaviour, IMove
{
    MoveType IMove.moveType => MoveType.Jump;
    Vector3 IMove.v => velocity;
    private Vector3 velocity;
    
    public float jumpImpulse;
    public Vector3 jumpDir = Vector3.up;
    public float jumpValue;
    public AnimationCurve jumpCurve;
    public float jumpCharge;
    public float jumpChargeMax;
    public float powerJumpMult;
    private bool grounded => PlayerMovement.grounded;

    private PlayerMovement playerMovement;
    public bool powerJumpMode => playerMovement.GetMoveData(MoveType.Planar).magnitude <= 0.1f;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        print("grounded: " + grounded);
        

        
        if (powerJumpMode) PowerJump();
        else Jump();
        
        
        
        if (jumpValue > 0)
        {
            jumpValue -= Time.deltaTime;
        }
    }

    void Jump()
    {
        jumpCharge = 0f;
        
        if(Input.GetButtonDown("Jump") && jumpValue <= 0 && grounded)
        {
            jumpValue = 1f;
        }
        velocity = Vector3.Lerp(Vector3.zero, jumpDir*jumpImpulse, jumpCurve.Evaluate(jumpValue));
    }

    void PowerJump()
    {
        if(Input.GetButtonDown("Jump") && jumpValue <= 0) // Pressed
        {
            jumpCharge = 0f;
        }
        
        if (grounded)
        {
            if (Input.GetButtonUp("Jump")) // Released
            {
                jumpValue = 1f;
            }

            if (Input.GetButton("Jump")) // Held
            {
                jumpCharge += Time.deltaTime;
                jumpCharge = math.clamp(jumpCharge, 0f, jumpChargeMax);
            }
        }
        
        
        velocity = Vector3.Lerp(Vector3.zero, jumpDir*(jumpImpulse + jumpCharge*powerJumpMult), jumpCurve.Evaluate(jumpValue));
    }



}
