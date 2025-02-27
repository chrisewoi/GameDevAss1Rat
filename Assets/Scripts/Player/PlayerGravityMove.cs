using UnityEngine;

public class PlayerGravityMove : MonoBehaviour, IMove
{
    private Vector3 velocity;
    MoveType IMove.moveType => MoveType.Gravity;
    Vector3 IMove.v => velocity;

    public float gravity;
    public float gravityMult;
    public float multAccelRate;
    
    
    void Start()
    {
        gravityMult = 1;
    }

    void Update()
    {
        if (PlayerMovement.grounded)
        {
            gravityMult = 1f;
        }
        else
        {
            gravityMult += multAccelRate * Time.deltaTime;
        }
        velocity = gravity * 9.8f * gravityMult * Vector3.down;
    }
}
