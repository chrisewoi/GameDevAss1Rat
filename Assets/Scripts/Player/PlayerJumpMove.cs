using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public float jumpBuffer;
    public float jumpBufferCurrent;

    public float coyoteTimeMax;
    public float coyoteTime;
    public bool coyoteJump;
    
    private bool grounded => PlayerMovement.grounded;

    private PlayerMovement playerMovement;
    public bool powerJumpMode => playerMovement.GetMoveData(MoveType.Planar).magnitude <= 0.1f;


    public Slider uiSlider;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        coyoteJump = false;
    }

    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            jumpBufferCurrent = jumpBuffer;
        }
        //print("grounded: " + grounded);

        if (grounded)
        {
            coyoteTime = coyoteTimeMax;
            if (!Input.GetButton("Jump"))
            {
                coyoteJump = true;
            }
        }
        else if (coyoteTime > 0f) 
        {
            coyoteTime = Mathf.Clamp(coyoteTime - Time.deltaTime, 0, coyoteTimeMax);
        }
        else
        {
            coyoteJump = false;
        }

        
        if (true /*powerJumpMode*/) PowerJump();
        else Jump();
        
        
        
        if (jumpValue > 0)
        {
            jumpValue -= Time.deltaTime;
            

            if (!Input.GetButton("Jump"))
            {
                jumpValue -= Time.deltaTime * 1f * jumpCharge;
            }
            jumpValue = Mathf.Clamp01(jumpValue);
        }
        else if (jumpCharge < 1)
        {
            jumpCharge = 0f;
        }

        uiSlider.value = jumpCurve.Evaluate(jumpCharge);
        jumpBufferCurrent -= Time.deltaTime;
        jumpBufferCurrent = math.clamp(jumpBufferCurrent, 0, jumpBuffer);
    }

    void Jump()
    {
        jumpCharge = 0f;
        
        if(Input.GetButtonDown("Jump") && jumpValue <= 0 && grounded) // ADD JUMPBUFFER HERE
        {
            jumpValue = 1f;
        }
        velocity = Vector3.Lerp(Vector3.zero, jumpDir*jumpImpulse, jumpCurve.Evaluate(jumpValue));
    }

    void PowerJump()
    {
        Jump2();
        velocity = Vector3.Lerp(Vector3.zero, jumpDir*(jumpImpulse + jumpCharge*powerJumpMult), jumpCurve.Evaluate(jumpValue));
        return;
        
        //////////
        /// Old Code
        //////////
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
        
        
        //velocity = Vector3.Lerp(Vector3.zero, jumpDir*(jumpImpulse + jumpCharge*powerJumpMult), jumpCurve.Evaluate(jumpValue));
    }

    public void Jump2()
    {
        if(Input.GetButtonDown("Jump") && coyoteJump) // Pressed
        {
            jumpValue = 1f;
            jumpCharge = 1f;
            coyoteJump = false;
            coyoteTime = coyoteTimeMax;
        }

        float jumpValueOnRelease = jumpValue;
        if (Input.GetButtonUp("Jump")) // Released
        {
            //jumpValue = 0f;
            //jumpValue = Mathf.Clamp01(jumpValue - Time.deltaTime * 30f * (jumpValue + 1f));
            jumpValue = Mathf.Clamp01(jumpValue - jumpValue * 0.3f);
        }

        if (Input.GetButton("Jump")) // Held
        {
            jumpCharge = Mathf.Clamp01(jumpCharge - Time.deltaTime);
        }
    }



}
