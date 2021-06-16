using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 moveToOffset;
    public float pause = 0.5f;
    public float speed = 1;

    Vector3 target;
    Vector3 originalPosition;
    float pauseTime;
    bool moveToTarget = true;

    Vector3 previousFramePos;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        pauseTime = Time.time;
        target = transform.position + (Vector3)moveToOffset;
    }

    // Update is called once per frame
    void Update()
    {

        velocity = (transform.position - previousFramePos) / Time.deltaTime;
        previousFramePos = transform.position;
        if (Time.time - pauseTime < pause) return;

        if (moveToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                moveToTarget = false;
                pauseTime = Time.time;
            }
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed);

            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
            {
                moveToTarget = true;
                pauseTime = Time.time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(velocity * 20);
            collision.gameObject.GetComponent<PlayerMovement>().horizontalControlScale = new Vector2(0, 1);
        }
    }

    private void OnDrawGizmos()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            if (Application.isPlaying)
            {
                UnityEditor.Handles.DrawDottedLine(originalPosition, target, 5f);
                Gizmos.color = new Color32(255, 255, 255, 50);
                Gizmos.DrawCube(target, (Vector3)renderer.size);
                Gizmos.DrawCube(originalPosition, (Vector3)renderer.size);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(target, (Vector3)renderer.size);
                Gizmos.DrawWireCube(originalPosition, (Vector3)renderer.size);
            } else
            {
                UnityEditor.Handles.DrawDottedLine(transform.position, transform.position + (Vector3)moveToOffset, 5f);
                Gizmos.color = new Color32(255, 255, 255, 50);
                Gizmos.DrawCube(transform.position + (Vector3)moveToOffset, (Vector3)renderer.size);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position + (Vector3)moveToOffset, (Vector3)renderer.size);
            }
        }
    }
}
