using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public Vector2 moveToOffset;
    public bool wakeOnTouch = true;
    bool awake = false;
    public float pause = 0.5f;
    public float speed = 1;

    Vector3 target;
    Vector3 originalPosition;
    float pauseTime;
    bool moveToTarget = true;

    Vector3 previousFramePos;
    Vector3 velocity;
    Vector3 velocityDelta;

    PlayerMovement plyrMovement;
    

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        awake = !wakeOnTouch;
        originalPosition = transform.position;
        
        target = transform.position + (Vector3)moveToOffset;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!awake) return;
        Vector3 newVel = ((transform.position - previousFramePos) / Time.deltaTime);
        velocityDelta = velocity - newVel;
        velocity = newVel;
        previousFramePos = transform.position;
        if (Time.time - pauseTime < pause) return;
        // Platform suddenly stopped
        
        

        Vector3 newTarget = transform.position;
        if (moveToTarget)
        {
            newTarget = target;
            if (Vector3.Distance(transform.position, newTarget) < 0.05f)
            {
                moveToTarget = false;
                pauseTime = Time.time;
                awake = false;
                
            }
        } 
        transform.position = Vector3.MoveTowards(transform.position, newTarget, speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!awake)
            {
                pauseTime = Time.time;
            }
            awake = wakeOnTouch;
            plyrMovement = collision.gameObject.GetComponent<PlayerMovement>();

            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(velocity * 50);
            //plyrMovement = collision.gameObject.GetComponent<PlayerMovement>();
            //plyrMovement.horizontalControlScale = new Vector2(0, 1);
            plyrMovement = null;
        }
    }
    public static void ResetPos(Transform transform, bool awake,Vector3 originalPosition)
    {
        awake = false;
        transform.position = originalPosition;
        Debug.Log(transform.position);
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
