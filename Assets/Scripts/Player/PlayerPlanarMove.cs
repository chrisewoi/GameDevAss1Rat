using System;
using UnityEngine;

public class PlayerPlanarMove : MonoBehaviour, IMove
{
    MoveType IMove.moveType => MoveType.Planar;
    Vector3 IMove.v => velocity;
    private Vector3 velocity;
    
    public float speed;
    

    public Vector2 moveInput => new Vector2(Input.GetAxis("Horizontal"),
                                            Input.GetAxis("Vertical"));

    private Camera camera;

    

    private void Awake()
    {
        camera = Camera.main;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Input to velocity
        velocity = 
            camera.transform.right * (moveInput.x * speed) + camera.transform.forward * (moveInput.y * speed);
        
        //makes it so you can't fly around like a damn bird
        velocity.y = 0f;
    }


}
