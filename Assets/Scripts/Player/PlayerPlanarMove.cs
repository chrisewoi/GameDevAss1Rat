using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerPlanarMove : MonoBehaviour, IMove
{
    MoveType IMove.moveType => MoveType.Planar;
    Vector3 IMove.v => velocity;
    private Vector3 velocity;

    public float speed;


    public Vector2 moveInput => new Vector2(Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));

    private new Camera camera;


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
        var input = moveInput;
        if (math.length(moveInput) > 1)
        {
            input = math.normalize(moveInput);
        }
        //input *= math.rcp(math.length(moveInput));
        velocity =
            camera.transform.right * (input.x * speed) + camera.transform.forward * (input.y * speed);

        //makes it so you can't fly around like a damn bird 
        velocity.y = 0f;
    }
}