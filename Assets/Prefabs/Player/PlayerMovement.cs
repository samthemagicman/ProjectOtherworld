using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning restore

public class PlayerMovement : MonoBehaviour
{
    public int walkSpeed = 20;
    [Tooltip("The lerp value to move to wanted velocity each frame")]
    public float velocityLerpValue = 0.4f;
    public LayerMask ignoreRaycastLayers;

    [Header("Jump Settings")]
    public float jumpCoyoteTime = 0.1f;
    public int jumpHeight = 50;

    [Tooltip("The max distance from the last platform that the player is allowed to be from to coyote jump")]
    public float maxCoyoteJumpDistance = 2f;

    [Header("Grabity Settings")]
    public float maxFallSpeed = 100;
    public float gravity = -100;

    [Header("Wall Jump Settings")]

    [InspectorName("Wall Slide Speed")]
    public float wallSlideSpeed = -15;

    [InspectorName("Coyote Time")]
    public float wallJumpCoyoteTime = 0.2f;

    [InspectorName("Move Delay after Jump")]
    [Tooltip("The time it takes before you can control your character again after a wall jump")]
    public float wallJumpMoveDelay = 0.15f;

    [Tooltip("The time it takes to unstick from a wall")]
    public float wallUnstickTime = 0.05f;

    public Vector2 wallJumpVelocity = new Vector2(20f, 40f);

    Rigidbody2D rb;
    Animator playerParentAnimator; // This is the animator that handles stretch/squad
    Animator animator; // This is the actual player's animation
    new SpriteRenderer renderer;
    ParticleSystem particles;

    Vector2 lastVerticalVelocity = new Vector2(0, 0);

    float jumpKeyPressedStamp = 0;
    bool isMovingTowardsWall = false;
    bool wallJumpKeyPressed = false;
    bool didJumpOffWallRecently = false;

    float mayJump = 0; 
    float mayWallJump = 0;

    float movingAwayFromWallStamp = 0;
    float startedMovingTimeStamp = 0;

    public static Vector2 lastGroundedPosition;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponentInChildren<ParticleSystem>();
        Animator[] animators = GetComponentsInChildren<Animator>();
        playerParentAnimator = animators[0];
        animator = animators[1];
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveHorizontalRaw = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveVerticalRaw = Input.GetAxisRaw("Vertical");

        bool isTouchingAnyWall = IsTouchingAnyWall();
        bool isGrounded = IsGrounded();
        bool isOnWall = isTouchingAnyWall && !isGrounded;

        if (isGrounded)
        {
            lastGroundedPosition = transform.position;
            mayJump = jumpCoyoteTime;
        }

        if (isTouchingAnyWall) mayWallJump = wallJumpCoyoteTime;

        isMovingTowardsWall = IsMovingTowardsWall(moveHorizontalRaw);
        bool isMovingAwayFromWall = IsMovingAwayFromWall(moveHorizontalRaw);

        if (isOnWall && isMovingAwayFromWall)
        {
            movingAwayFromWallStamp += Time.deltaTime;
        } else if (!isMovingAwayFromWall)
        {
            movingAwayFromWallStamp = 0;
        }

        if (Mathf.Abs(moveHorizontalRaw) > 0)
        {
            startedMovingTimeStamp += Time.deltaTime;
        }
        else if (!isMovingAwayFromWall)
        {
            startedMovingTimeStamp = 0;
        }

        #region Handle walking and moving in air
        if (!didJumpOffWallRecently) //If they just jumped off a wall, they can't control their character
        {
            Vector2 wantedVelocity;
            if (isGrounded)
            {
                wantedVelocity = new Vector2(moveHorizontalRaw, 0) * walkSpeed + new Vector2(0, rb.velocity.y);
            } else // Moving in air
            {
                // If the player isn't already moving FASTER than the move speed, and they
                
                if ((rb.velocity.x <  walkSpeed && moveHorizontalRaw > 0) ||
                    (rb.velocity.x > -walkSpeed && moveHorizontalRaw < 0))
                {
                    wantedVelocity = new Vector2(moveHorizontalRaw, 0) * walkSpeed + new Vector2(rb.velocity.x, rb.velocity.y);
                }
                else
                {
                    wantedVelocity = new Vector2(rb.velocity.x * 0.6f, rb.velocity.y);
                }
            }

            if (isOnWall && movingAwayFromWallStamp < wallUnstickTime) // on wall
            {
                wantedVelocity *= new Vector2(0, 1);
            }

            rb.velocity = Vector2.Lerp(rb.velocity, wantedVelocity, velocityLerpValue);
        }
        #endregion

        #region Jump
        if ((mayJump > 0 && jumpKeyPressedStamp > 0 && Vector2.Distance(lastGroundedPosition, transform.position) < maxCoyoteJumpDistance)
            || isGrounded && jumpKeyPressedStamp > 0)// && !isTouchingAnyWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);//rb.velocity * new Vector2(1, 0) + (new Vector2(0, moveVerticalRaw) * jumpHeight);
            mayJump = 0;
        }
        #endregion

        #region Gravity and wall sliding
        if (isTouchingAnyWall && !isMovingTowardsWall)
        {
            // If they're touching a wall, they're not moving towards it, and they're going UP the wall, then fake some friction (slow them down sliding up the wall)
            if (rb.velocity.y > 0)
            {
                rb.velocity *= new Vector2(1, 0.92f);
            }

            //Basically if we're touching a wall and we're not moving towards a wall
            //Apply slower gravity
            //rb.AddForce(new Vector2(0, wallSlidingGravity)); // Old way of adding slow gravity
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, wallSlideSpeed), 0.1f);
        }
        else if (isTouchingAnyWall && isMovingTowardsWall)
        {
            rb.velocity = rb.velocity * new Vector2(1, 0.7f); // If space was not down, and we're touching any wall, and we're moving towards a wall, slow down the y velocity to 0
        }
        else // Apply gravity
        {
            rb.AddForce(new Vector2(0, gravity));
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallSpeed, Mathf.Infinity));
        }
        #endregion

        #region Wall jumping
        if (mayWallJump > 0 && wallJumpKeyPressed && !isGrounded) // Wall jump
        {
            var wallJumpXVelocity = IsTouchingLeftWall() == true ? wallJumpVelocity.x : -wallJumpVelocity.x;
            if (mayWallJump != wallJumpCoyoteTime)
            {
                wallJumpXVelocity = moveHorizontalRaw > 0 == true ? wallJumpVelocity.x : -wallJumpVelocity.x;
            }
            
            rb.velocity = new Vector2(wallJumpXVelocity, wallJumpVelocity.y);
            wallJumpKeyPressed = false;
            didJumpOffWallRecently = true;
            Invoke("SetJumpedOffWallToFalse", wallJumpMoveDelay);
            mayWallJump = 0;
        }
        #endregion

        UpdateParticles(moveHorizontalRaw);
        UpdateAnimation(moveHorizontal);

        jumpKeyPressedStamp -= Time.deltaTime / Time.timeScale;
        mayJump -= Time.deltaTime / Time.timeScale;
        mayWallJump -= Time.deltaTime / Time.timeScale;
        lastVerticalVelocity = rb.velocity;
    }

    bool IsMovingTowardsWall(float moveHorizontalRaw)
    {
        if ((IsTouchingLeftWall() && moveHorizontalRaw < 0) ||
            (IsTouchingRightWall() && moveHorizontalRaw > 0))
        {
            return true;

        }
        else
        {
            return false;
        }
    }


    bool IsMovingAwayFromWall(float moveHorizontalRaw)
    {
        if ((IsTouchingLeftWall() && moveHorizontalRaw > 0) ||
            (IsTouchingRightWall() && moveHorizontalRaw < 0))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    void UpdateAnimation(float moveHorizontal)
    {
        bool isGrounded = IsGrounded();
        if (animator.GetBool("Falling") && isGrounded) // If they were falling and they're now grounded
        {
            //For some reason, lastVerticalVelocity is sometimes 0. I should've wrote down why this is important
            animator.SetFloat("LandedVelocity", lastVerticalVelocity.y);
            playerParentAnimator.SetTrigger("Fell");
        }

        animator.SetBool("Falling", !isGrounded && !IsTouchingRightWall() && !IsTouchingLeftWall());
        animator.SetBool("Walking", Mathf.Abs(moveHorizontal) > 0);
        animator.SetFloat("Velocity", rb.velocity.x / walkSpeed);
        animator.SetFloat("AbsoluteVelocity", Mathf.Abs(moveHorizontal));
        animator.SetFloat("VerticalVelocity", rb.velocity.y);

        var t = 0;
        if (!isGrounded)
        {
            if (IsTouchingRightWall())
            {
                t = 1;
            }
            else if (IsTouchingLeftWall())
            {
                t = -1;
            }
        }
        animator.SetFloat("OnWallFloat", t);
        animator.SetBool("OnWall", t != 0);
    }

    void UpdateParticles(float moveHorizontalRaw)
    {
        if (IsGrounded() && moveHorizontalRaw != 0) particles.Play();
        else particles.Stop();

        if (rb.velocity.x < 0)
        {
            var shape = particles.shape;
            shape.position = new Vector3(1.3f, -0.9f, 5);
        }
        else if (rb.velocity.x > 0)
        {
            var shape = particles.shape;
            shape.position = new Vector3(-1.3f, -0.9f, 5);
        }
    }

    void SetJumpedOffWallToFalse()
    {
        didJumpOffWallRecently = false;
    }

    private void LateUpdate()
    {
        if ((IsTouchingAnyWall() || mayWallJump > 0) && Input.GetButtonDown("Vertical")) wallJumpKeyPressed = true;
        if (Input.GetButtonDown("Vertical")) jumpKeyPressedStamp = 0.2f; ;
        
    }

    bool IsGrounded() {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position, new Vector2(1.9f, 0.05f), 0, -Vector2.up, 0, ~ignoreRaycastLayers).collider != null;
    }

    bool IsTouchingRightWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.8f), 0, Vector2.right, 0, ~ignoreRaycastLayers).collider != null;
    }

    bool IsTouchingLeftWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.8f), 0, Vector2.right, 0, ~ignoreRaycastLayers).collider != null;
    }

    bool IsTouchingAnyWall()
    {
        return IsTouchingRightWall() || IsTouchingLeftWall();
    }

    //Draw some little gizmos to see the colliders for Grounded, Left and Right wall
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector2(1.9f, 0.05f));

        //Right wall
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.8f)) ;

        //Left wall
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.8f));
    }
}
