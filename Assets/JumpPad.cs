using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public enum Face { up, down, left, right }
    public float force;
    public Face face;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 vel = Vector2.zero;

            switch(face)
            {
                case Face.right:
                    vel = transform.right;
                    break;
                case Face.up:
                    vel = transform.up;
                    break;
                case Face.down:
                    vel = transform.up * -1;
                    break;
                case Face.left:
                    vel = transform.right * -1;
                    break;
            }
            if (collision.contacts[0].normal != -vel) return;
            animator.SetTrigger("Bounce");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce( vel * force );
            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.Reflect(collision.gameObject.GetComponent<Rigidbody2D>().velocity, collision.contacts[0].normal) * force;
            collision.gameObject.GetComponent<PlayerMovement>().horizontalControlScale = new Vector2(0, 1);
            collision.gameObject.GetComponent<PlayerMovement>().horizontalControlScale = new Vector2(0.1f, 1);
            //collision.gameObject.GetComponent<PlayerMovement>().Fling(vel * force, 0.4f);
        }
    }
}
