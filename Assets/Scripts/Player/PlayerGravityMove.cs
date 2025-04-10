using UnityEngine;

public class PlayerGravityMove : MonoBehaviour, IMove
{
    private static Vector3 velocity;
    MoveType IMove.moveType => MoveType.Gravity;
    Vector3 IMove.v => velocity;

    public static float gravity;
    public static float gravityMult;
    public static float butterGravityMult;
    public static float multAccelRate;
    
    
    void Start()
    {
        gravity = 2.5f;
        gravityMult = 1;
        multAccelRate = 1f;
        butterGravityMult = 1;
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
        velocity = gravity * 9.8f * gravityMult * butterGravityMult * Vector3.down;
    }

    public static Vector3 GetGravityVelocity()
    {
        return velocity;
    }

    public static void SetButterGravityMult(float value)
    {
        butterGravityMult = value;
    }

    public static float GetButterGravityMult()
    {
        return butterGravityMult;
    }
}
