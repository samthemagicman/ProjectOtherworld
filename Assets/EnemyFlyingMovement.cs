using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovement : MonoBehaviour
{
    public float playerDetectionRadius = 25;
    public float speed = 15;

    Enemy enemyHandler;

    GameObject player;

    SpriteRenderer renderer;
    Animator animator;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    bool attacking = false;
    bool knockback = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        enemyHandler = GetComponent<Enemy>();

        enemyHandler.onDamaged.AddListener(() =>
        {
            knockback = true;

            if (player)
            {
                rb.velocity = -(player.transform.position - transform.position).normalized * 80;
                Invoke("SetKnockbackToFalse", 0.3f);
            }
        });

        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = playerDetectionRadius;

        SetAttackingToFalse();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
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

    // Update is called once per frame
    void Update()
    {
        if (knockback) return;

        animator.SetFloat("HorizontalVelocity", rb.velocity.x);

        if (rb.velocity.x < -0.02)
        {
            renderer.flipX = true;
        } else if (rb.velocity.x > 0.02)
        {
            renderer.flipX = false;
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("Flipped", renderer.flipX);
    }

    void SetKnockbackToFalse()
    {
        knockback = false;
    }

    void SetAttackingToFalse()
    {
        animator.SetBool("Attacking", false);
        attacking = false;
        Invoke("SetAttackingToTrue", Random.Range(2, 5));
    }

    void SetAttackingToTrue()
    {
        animator.SetBool("Attacking", true);
        attacking = true;
        Invoke("SetAttackingToFalse", 3f);
    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            rb.velocity = rb.velocity * 0.9f;
            return;
        }


        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            if (attacking)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, directionToPlayer * speed, 0.3f);

            }
            else
            {
                directionToPlayer = (playerPosition - transform.position - directionToPlayer * 8).normalized;

                if (Vector3.Distance(transform.position, playerPosition) > 9)
                {
                    rb.velocity = Vector3.Lerp(rb.velocity, directionToPlayer * speed, 0.08f);
                }
                else
                {

                    if (transform.position.y < playerPosition.y + 8)
                    {
                        directionToPlayer = (playerPosition - transform.position).normalized;
                        directionToPlayer = (playerPosition + new Vector3(0, 8, 0) - transform.position - directionToPlayer * 8).normalized;

                        rb.velocity = Vector3.Lerp(rb.velocity, directionToPlayer * speed, 0.08f);
                    }
                    else
                    {
                        rb.velocity = new Vector3(0, 0, 0);


                        if (transform.position.x > playerPosition.x)
                        {
                            renderer.flipX = true;
                        }
                        else
                        {
                            renderer.flipX = false;
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
