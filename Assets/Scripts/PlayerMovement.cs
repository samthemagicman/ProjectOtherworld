using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 20;
    public int jumpHeight = 50;

    float distToGround = 0f;
    Rigidbody2D rb;
    Animator playerParentAnimator;
    Animator animator;
    new SpriteRenderer renderer;
    ParticleSystem particles;

    bool flipped;
    bool sliding = false;

    Vector2 lastVerticalVelocity = new Vector2(0, 0);

    bool isMovingTowardsWall = false;

    bool spaceWasDown = false;
    bool jumpedOffWall = false;

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

        if (!jumpedOffWall)
        {
            if (IsGrounded())
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveHorizontalRaw, 0) * speed + new Vector2(0, rb.velocity.y), 0.2f);
            } else
            {
                if ((rb.velocity.x < speed && moveHorizontalRaw > 0) || (rb.velocity.x > -speed && moveHorizontalRaw < 0))
                {
                    rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveHorizontalRaw, 0) * speed + new Vector2(rb.velocity.x, rb.velocity.y), 0.2f);
                }
                else
                {
                    rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, rb.velocity.y), 0.2f);
                }
            }
        }

        if (IsGrounded() && !IsTouchingLeftWall() && !IsTouchingRightWall())
        {
            rb.velocity = rb.velocity * new Vector2(1, 0) + (new Vector2(0, moveVerticalRaw) * jumpHeight);
        }

        if (animator.GetBool("Falling") && IsGrounded()) // If they were falling and they're now grounded
        {
            //For some reason, lastVerticalVelocity is sometimes 0  
            animator.SetFloat("LandedVelocity", lastVerticalVelocity.y);
            playerParentAnimator.SetTrigger("Fell");
        }

        if (IsGrounded() && moveHorizontalRaw != 0) particles.Play();
        else particles.Stop();

        if (rb.velocity.x < 0)
        {
            var shape = particles.shape;
            shape.position = new Vector3(1.3f, -0.9f, 5);
        } else if (rb.velocity.x > 0)
        {
            var shape = particles.shape;
            shape.position = new Vector3(-1.3f, -0.9f, 5);
        }

        #region Animation
        //Handle a bunch of animation 
        animator.SetBool("Falling", !IsGrounded() && !IsTouchingRightWall() && !IsTouchingLeftWall());
        animator.SetBool("Walking", Mathf.Abs(moveHorizontal) > 0);
        animator.SetFloat("Velocity", rb.velocity.x / speed);
        animator.SetFloat("AbsoluteVelocity", Mathf.Abs(moveHorizontal));
        animator.SetFloat("VerticalVelocity", rb.velocity.y);

        var t = 0;
        if (IsTouchingRightWall())
        {
            t = 1;
        } else if (IsTouchingLeftWall())
        {
            t = -1;
        }
        animator.SetFloat("OnWallFloat", t);
        animator.SetBool("OnWall", IsTouchingLeftWall() || IsTouchingRightWall());
        #endregion

        if ((IsTouchingLeftWall() && moveHorizontalRaw < 0) || (IsTouchingRightWall() && moveHorizontalRaw > 0))
        {
            isMovingTowardsWall = true;

        } else
        {
            isMovingTowardsWall = false;
        }

        var isTouchingAnyWall = IsTouchingLeftWall() || IsTouchingRightWall();

        if (isTouchingAnyWall && !isMovingTowardsWall)
        {
            if (rb.velocity.y > 0) // If they jumped and then they're touching a wall, slowly decrease the Y velocity
            {
                rb.velocity *= new Vector2(1, 0.92f);
            }
            rb.AddForce(new Vector2(0, -15)); // If we're touching a wall and we're not moving towards a wall, slowly slide the player down

        }
        else if (isTouchingAnyWall && isMovingTowardsWall)
        {
            rb.velocity = rb.velocity * new Vector2(1, 0.85f); // If space was not down, and we're touching any wall, and we're moving towards a wall, slow down the y velocity to 0
        }
        else
        {
            rb.AddForce(new Vector2(0, -100));
        }

        if (isTouchingAnyWall && spaceWasDown)
        {
            var wallJumpXVelocity = IsTouchingLeftWall() == true ? 20 : -20;
            rb.velocity = new Vector2(wallJumpXVelocity, 40);
            spaceWasDown = false;
            jumpedOffWall = true;
            Invoke("SetJumpedOffWallToFalse", 0.15f);
        }
        lastVerticalVelocity = rb.velocity;


        if (Input.GetMouseButton(1))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.03f, 0.1f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.12f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
    }

    void SetJumpedOffWallToFalse()
    {
        jumpedOffWall = false;
    }

    private void LateUpdate()
    {
        if ((IsTouchingLeftWall() || IsTouchingRightWall()) && Input.GetKeyDown(KeyCode.Space)) spaceWasDown = true;
    }

    bool IsGrounded() {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position, new Vector2(2f, 0.05f), 0, -Vector2.up, distToGround).collider != null;
    }

    bool IsTouchingRightWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.9f), 0, Vector2.right, distToGround).collider != null;
    }

    bool IsTouchingLeftWall()
    {
        //return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f).collider != null;
        return Physics2D.BoxCast(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.9f), 0, Vector2.right, distToGround).collider != null;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector2(2f, 0.05f));

        //Right wall
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(1, 1, 0), new Vector2(0.05f, 1.9f)) ;

        //Left wall
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(-1, 1, 0), new Vector2(0.05f, 1.9f));
    }
}
