using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundMovement : MonoBehaviour
{
    private enum Direction { Left, Right }
    Direction currentDirection = Direction.Right;
    private enum States { Idle, Walking, Attacking, Knockback }
    private States currentState = States.Idle;

    public AttackHitbox attackHitbox;

    public float playerDetectionRadius = 25;
    public float walkSpeed = 8;
    public float runSpeed = 15;
    public float lungeSpeed = 40;

    public float edgeDetectionDistance = 5;
    public LayerMask edgeDetectionIgnoreLayers;

    GameObject player;
    bool playerInRange;

    Enemy enemyHandler;

    SpriteRenderer renderer;
    Animator animator;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    float lastAttack = 0;

    bool lunging = false;

    bool leftPunch;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        enemyHandler = GetComponent<Enemy>();

        enemyHandler.onDamaged.AddListener(() =>
        {
            currentState = States.Knockback;

            if (player)
            {
                rb.velocity = -(player.transform.position - transform.position).normalized * 50;
                animator.SetTrigger("Hurt");
                Invoke("SetKnockbackToFalse", 0.3f);
            }
        });

        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = playerDetectionRadius / transform.localScale.x;

        attackHitbox.onPlayerHit.AddListener(onPlayerHit);
    }

    //Detect if the player is close by
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = null;
        }
    }

    void onPlayerHit(GameObject player)
    {
        if (lunging)
        {
            //player.GetComponent<Rigidbody2D>().velocity = (new Vector2((transform.position.normalized.x - player.transform.position.normalized.x) * 500, 30));
            float xVel = transform.position.x < player.transform.position.x ? 1 : -1;
            player.GetComponent<PlayerMovement>().Fling(new Vector2(xVel * 30, 30), 0.35f);
            player.GetComponent<PlayerHealth>().Damage(1);
            rb.velocity = new Vector2(-xVel, 0) * 30;
            currentState = States.Knockback;
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        } else
        {
            playerInRange = Vector2.Distance(player.transform.position, transform.position) < playerDetectionRadius;
        }
        if (currentState == States.Walking)
        {
            animator.SetFloat("AbsoluteHorizontalVelocity", Mathf.Abs(rb.velocity.x) / runSpeed);
        } else if (currentState == States.Idle)
        {
            animator.SetFloat("AbsoluteHorizontalVelocity", 0);
        }
    }

    void SetKnockbackToFalse()
    {
        lunging = false;
        currentState = States.Idle;
    }

    void SetAttackingToFalse()
    {
        //animator.SetBool("Attacking", false);
        //Invoke("SetAttackingToTrue", Random.Range(2, 5));
    }

    void SetAttackingToTrue()
    {
        //animator.SetBool("Attacking", true);
        //Invoke("SetAttackingToFalse", 3f);
    }

    void StartLunge()
    {
        lunging = true;
    }

    void StopLunge()
    {
        lunging = false;
        currentState = States.Idle;
    }


    void SetPunchingToFalse()
    {
    }

    private void Punch()
    {
        currentState = States.Attacking;

        Invoke("SetPunchingToFalse", 0.7f);
        if (leftPunch)
        {
            animator.SetTrigger("LeftPunch");
        }
        else
        {
            animator.SetTrigger("RightPunch");
        }
        leftPunch = !leftPunch;
    }

    bool WillFallOffEdge(Direction direction)
    {
        Vector3 offset = new Vector3(edgeDetectionDistance, 0);

        if (direction == Direction.Right)
        {
            RaycastHit2D raycastRight = Physics2D.Raycast(transform.position + offset + new Vector3(0, 2), Vector2.down, 5, ~edgeDetectionIgnoreLayers);
            if (raycastRight)
            {
                return false;
            }
        } else
        {
            RaycastHit2D raycastLeft = Physics2D.Raycast(transform.position - offset + new Vector3(0, 2), Vector2.down, 5, ~edgeDetectionIgnoreLayers);
            if (raycastLeft)
            {
                return false;
            }
        }

        return true;
    }

    void SwapFacingDirectionTo(Direction direction)
    {
        if (direction == Direction.Left)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        currentDirection = direction;

    }

    void Move()
    {
        if (playerInRange && player)
        {
            Vector3 playerPosition = player.transform.position;

            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            if (directionToPlayer.x > 0)
            {
                SwapFacingDirectionTo(Direction.Right);
            }
            else
            {
                SwapFacingDirectionTo(Direction.Left);
            }

            directionToPlayer = (playerPosition - transform.position - directionToPlayer * 7).normalized;

            if (Mathf.Abs(transform.position.x - player.transform.position.x) < 9 && Time.realtimeSinceStartup - lastAttack > 1.5f)
            {
                lastAttack = Time.realtimeSinceStartup;
                Punch();
            }
            else if (Mathf.Abs(transform.position.x - player.transform.position.x) > 8)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(directionToPlayer.x * runSpeed, rb.velocity.y), 0.08f);
                currentState = States.Walking;
            } else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                currentState = States.Idle;

            }
        } else
        {
            if (currentDirection == Direction.Right)
            {
                if (!WillFallOffEdge(Direction.Right))
                {
                    rb.velocity = new Vector3(walkSpeed, rb.velocity.y);
                    currentState = States.Walking;
                } else
                {
                    SwapFacingDirectionTo(Direction.Left);
                }
            } else
            {
                if (!WillFallOffEdge(Direction.Left))
                {
                    rb.velocity = new Vector3(-walkSpeed, rb.velocity.y);
                    currentState = States.Walking;
                }
                else
                {
                    SwapFacingDirectionTo(Direction.Right);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentState == States.Knockback)
        {
            bool willFallOffEdge = WillFallOffEdge(Direction.Left) || WillFallOffEdge(Direction.Right);

            if (!willFallOffEdge)
            {
                rb.velocity = rb.velocity * 0.9f;
            } else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

        } else if (lunging)
        {
            if (WillFallOffEdge(currentDirection))
            {
                rb.velocity = new Vector3(0, rb.velocity.y);
            } else
            {
                rb.velocity = new Vector3( (currentDirection == Direction.Left ? -1 : 1) * lungeSpeed, rb.velocity.y);
            }
        }
        else if (currentState == States.Attacking)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            Move();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Vector3 offset1 = new Vector3(edgeDetectionDistance, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position - offset1 + new Vector3(0, -2), transform.position - offset1 + new Vector3(0, 2));
        Gizmos.DrawLine(transform.position + offset1 + new Vector3(0, -2), transform.position + offset1 + new Vector3(0, 2));

    }
}
