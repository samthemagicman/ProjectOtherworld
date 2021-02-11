
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 20;
    public int jumpHeight = 50;

    public float wallSlidingGravity = -15;
    public float gravity = -100;

    public float jumpCoyoteTime = 0.1f;
    public float wallJumpCoyoteTime = 0.2f;

    Rigidbody2D rb;
    Animator playerParentAnimator; // This is the animator that handles stretch/squad
    Animator animator; // This is the actual player's animation
    new SpriteRenderer renderer;
    ParticleSystem particles;

    Vector2 lastVerticalVelocity = new Vector2(0, 0);

    bool isMovingTowardsWall = false;
    bool jumpKeyWasDown = false;
    bool didJumpOffWallRecently = false;

    float mayJump = 0;
    float mayWallJump = 0;


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

        bool isTouchingAnyWall = IsTouchingLeftWall() || IsTouchingRightWall();
        bool isGrounded = IsGrounded();
        if (isGrounded) mayJump = wallJumpCoyoteTime;
        if (isTouchingAnyWall) mayWallJump = wallJumpCoyoteTime;

        isMovingTowardsWall = IsMovingTowardsWall(moveHorizontalRaw);

        #region Handle walking and moving in air
        if (!didJumpOffWallRecently) //If they just jumped off a wall, they can't control their character
        {
            Vector2 wantedVelocity;
            if (isGrounded)
            {
                wantedVelocity = new Vector2(moveHorizontalRaw, 0) * speed + new Vector2(0, rb.velocity.y);
            }
            else
            {
                // If the player isn't already moving FASTER than the move speed, and they

                if ((rb.velocity.x < speed && moveHorizontalRaw > 0) ||
                    (rb.velocity.x > -speed && moveHorizontalRaw < 0))
                {
                    wantedVelocity = new Vector2(moveHorizontalRaw, 0) * speed + new Vector2(rb.velocity.x, rb.velocity.y);
                }
                else
                {
                    wantedVelocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }
            }
            rb.velocity = Vector2.Lerp(rb.velocity, wantedVelocity, 0.2f);
        }
        #endregion

        #region Jump
        if (mayJump > 0 && moveVerticalRaw > 0)// && !isTouchingAnyWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveVerticalRaw * jumpHeight);//rb.velocity * new Vector2(1, 0) + (new Vector2(0, moveVerticalRaw) * jumpHeight);
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
            rb.AddForce(new Vector2(0, wallSlidingGravity)); // If we're touching a wall and we're not moving towards a wall, slowly slide the player down

        }
        else if (isTouchingAnyWall && isMovingTowardsWall)
        {
            rb.velocity = rb.velocity * new Vector2(1, 0.85f); // If space was not down, and we're touching any wall, and we're moving towards a wall, slow down the y velocity to 0
        }
        else // Apply gravity
        {
            rb.AddForce(new Vector2(0, gravity));
        }
        #endregion

        #region Wall jumping
        if (mayWallJump > 0 && jumpKeyWasDown && !isGrounded) // Wall jump
        {
            var wallJumpXVelocity = IsTouchingLeftWall() == true ? 20 : -20;
            if (mayWallJump != wallJumpCoyoteTime)
            {
                wallJumpXVelocity = moveHorizontalRaw > 0 == true ? 20 : -20;
            }

            rb.velocity = new Vector2(wallJumpXVelocity, 40);
            jumpKeyWasDown = false;
            didJumpOffWallRecently = true;
            Invoke("SetJumpedOffWallToFalse", 0.15f);
            mayWallJump = 0;
        }
        #endregion

        UpdateParticles(moveHorizontalRaw);
        UpdateAnimation(moveHorizontal);

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
        animator.SetFloat("Velocity", rb.velocity.x / speed);
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
        if ((IsTouchingLeftWall() || IsTouchingRightWall() || mayWallJump > 0) && Input.GetKeyDown(KeyCode.Space)) jumpKeyWasDown = true;
    }

    bool IsGrounded()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position, new Vector2(1.9f, 0.05f), 0, -Vector2.up, 0).collider != null;
    }

    bool IsTouchingRightWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.8f), 0, Vector2.right, 0).collider != null;
    }

    bool IsTouchingLeftWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.8f), 0, Vector2.right, 0).collider != null;
    }

    //Draw some little gizmos to see the colliders for Grounded, Left and Right wall
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector2(1.9f, 0.05f));

        //Right wall
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.8f));

        //Left wall
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.8f));
    }
}
