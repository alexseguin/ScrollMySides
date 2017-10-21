using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float acceleratioTimeAirborne = .2f;
    public float acceleratioTimeGrounded = .1f;
    public float moveSpeed = 6;
    public float wallSlideSpeedMax = 3;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    float jumpVelocity;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        print("Gravity: " + gravity + " && JumpVelocity: " + jumpVelocity);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        int wallDirX = (controller.collisions.left) ? -1 : 1;
        bool wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
        }

        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if(wallDirX == input.x)
                {
                    print(wallDirX);
                    print(input.x);
                    print(wallJumpClimb.x);
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                } else if(input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y; 
                } else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if(controller.collisions.below)
                velocity.y = jumpVelocity;
        }
            

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ?acceleratioTimeGrounded : acceleratioTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
