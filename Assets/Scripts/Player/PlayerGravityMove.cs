using UnityEngine;

public class PlayerGravityMove : MonoBehaviour, IMove
{
    private static Vector3 velocity;
    MoveType IMove.moveType => MoveType.Gravity;
    Vector3 IMove.v => velocity;

    public static float gravity;
    public static float gravityMult;
    public static float multAccelRate;
    
    
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

    public static Vector3 GetGravityVelocity()
    {
        return velocity;
    }

    public static void SetGravityMult(float value)
    {
        gravityMult = value;
    }

    public static float GetGravityMult()
    {
        return gravityMult;
    }
}
