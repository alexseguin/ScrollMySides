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
    float charSpeed;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    Animator animator;

    public float lastDirection;
    private bool running;
    private bool crouching;
    private bool walking;
    private bool jumping;

    // Use this for initialization
    void Start () {
        lastDirection = 1;
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        charSpeed = moveSpeed;
        gravity = -(2 * jumpHeight) / (Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
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
        else
        {
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

        

        if (Input.GetKey(KeyCode.LeftShift) && controller.collisions.below)
        {
            running = true;
            crouching = false;
            charSpeed = moveSpeed * 2;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift)&& controller.collisions.below) {
            crouching = true;
            running = false;
            charSpeed = moveSpeed / 3;
        }
        else
        {
            charSpeed = moveSpeed;
            running = false;
            crouching = false;
        }

        if (input.x != 0 && controller.collisions.below)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }


        float targetVelocityX = input.x * charSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ?acceleratioTimeGrounded : acceleratioTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (input.x != 0)
            lastDirection = Mathf.Sign(input.x);

        animator.SetBool("WallslideLeft", (wallSliding && controller.collisions.left));
        animator.SetBool("WallslideRight", (wallSliding && controller.collisions.right));
        animator.SetBool("Walking", walking);
        animator.SetBool("Crouching", crouching);
        animator.SetBool("Running", running && input.x != 0);
        animator.SetBool("Jumping", !controller.collisions.below);
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("DirY", Mathf.Sign(velocity.y));
        animator.SetFloat("LastDir", lastDirection);
    }
}
