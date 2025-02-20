using UnityEngine;

public class PlayerGravityMove : MonoBehaviour, IMove
{
    private Vector3 velocity;
    MoveType IMove.moveType => MoveType.Gravity;
    Vector3 IMove.v => velocity;

    public float gravity;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        velocity = gravity * 9.8f * Vector3.down;
    }
}
