using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public static bool grounded;

    public static float timeUngrounded;
    public static float timeToUnground;

    private Vector3 finalVelocity;
    private static bool isFalling;


    private IMove[] moveInterfaces;
    public struct IMoveData
    {
        public MoveType moveType;
        public Vector3 v;
    }


    public float butterPlanarVelocityMult;
    private bool onButter => PlayerButterSlideMove.IsSliding();
    
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        moveInterfaces = GetComponents<IMove>();
        
    }

    void Start()
    {
        
    }

    void Update()
    {

        finalVelocity = Vector3.zero;
        characterController.radius = 0.5f + PlayerButterSlideMove.ButterHeight();
        
        foreach (var move in moveInterfaces)
        {
            //print(move.moveType + " velocity: " + move.v);
            finalVelocity += move.v;
        }

        if (onButter)
        {
            Vector3 velocity = finalVelocity;
            finalVelocity *= butterPlanarVelocityMult;
            finalVelocity.y = velocity.y;
        }
        isFalling = finalVelocity.y < 0f && !GroundCheck.isGrounded() ? true : false;
        characterController.Move(finalVelocity*Time.deltaTime);


        if (timeUngrounded > timeToUnground)
        {
            grounded = false;
        }

        if (GroundCheck.isGrounded())
        {
            timeUngrounded = 0f;
            grounded = true;
        }

        timeUngrounded += Time.deltaTime;
    }
    public IMoveData[] GetMovementData()
    {
        var moveData = new IMoveData[moveInterfaces.Length];
        for (int i = 0; i < moveInterfaces.Length; i++)
        {
            moveData[i] = new IMoveData
            {
                moveType = moveInterfaces[i].moveType,
                v = moveInterfaces[i].v,
            };
        }
        return moveData;
    }

    public Vector3 GetMoveData(MoveType moveType)
    {
        foreach (var item in moveInterfaces)
        {
            if (item.moveType == moveType)
            {
                return item.v;
            }
        }

        return Vector3.positiveInfinity;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            timeUngrounded = 0f;
            grounded = true;
        }
    }

    public Vector3 GetVelocity()
    {
        return finalVelocity;
    }

    public static bool IsFalling()
    {
        return isFalling;
    }
    
}
public interface IMove
{
    public MoveType moveType { get; }
    public Vector3 v { get; }
}

public enum MoveType
{
    Planar, Jump, ButterSlide, Gravity
}