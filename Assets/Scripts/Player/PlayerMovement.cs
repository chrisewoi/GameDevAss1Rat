using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    public bool grounded;

    public float timeUngrounded;
    public float timeToUnground;

    


    private IMove[] moveInterfaces;
    public struct IMoveData
    {
        public MoveType moveType;
        public Vector3 v;
    }
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
        var finalVelocity = Vector3.zero;
        
        
        foreach (var move in moveInterfaces)
        {
            finalVelocity += move.v;
        }
        characterController.Move(finalVelocity*Time.deltaTime);


        if (timeUngrounded > timeToUnground)
        {
            grounded = false;
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