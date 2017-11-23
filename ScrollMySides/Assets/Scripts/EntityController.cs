using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

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
    float charSpeed;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();
        charSpeed = moveSpeed;
        gravity = -(2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector2 input = aiInput();
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
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
                velocity.y = jumpVelocity;
        }

        if (Input.GetKey(KeyCode.LeftShift) && controller.collisions.below)
        {
            charSpeed = moveSpeed * 2;
        }
        else
        {
            charSpeed = moveSpeed;
        }

        float targetVelocityX = input.x * charSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acceleratioTimeGrounded : acceleratioTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }



    private Vector2 aiInput()
    {
        Vector2 aiMovement = new Vector2(0,0);
        int startDir = 0;
        if (startDir < 1 && controller.OnEdge(velocity))
            aiMovement.x += moveSpeed;
        else
            aiMovement.x -= moveSpeed;

        return aiMovement;
    }
}
