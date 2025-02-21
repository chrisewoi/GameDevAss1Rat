using UnityEngine;

public class PlayerButterSlideMove : MonoBehaviour, IMove
{
    private Vector3 velocity;
    MoveType IMove.moveType => MoveType.ButterSlide;
    Vector3 IMove.v => velocity;

    public float speed;
    public bool sliding;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


}
