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

    //rumble before fall effect. 
    SpriteRenderer sprite;
    private bool flipx = true;
    [Range(1,10)]
    public float rumbleTimeMod = 1.8f;

    Vector3 previousFramePos;
    Vector3 velocity;
    Vector3 velocityDelta;

    PlayerMovement plyrMovement;
    

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        target = transform.position + (Vector3)moveToOffset;
        rb = GetComponent<Rigidbody2D>();
        PlayerDeathHandler.onDeath.AddListener(ResetPos);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!awake) return;
        if (Time.time - pauseTime < pause)
        {
            if(!rumbling) StartCoroutine("Rumble");
            return;
        }
        
        Vector3 newVel = ((transform.position - previousFramePos) / Time.deltaTime);
        velocityDelta = velocity - newVel;
        velocity = newVel;
        previousFramePos = transform.position;
        
        
        
        Vector3 newTarget = transform.position;
        if (moveToTarget)
        {
            newTarget = target;
            if (Vector3.Distance(transform.position, newTarget) < 0.05f)
            {
                moveToTarget = false;
                pauseTime = Time.time;
                awake = false;
                wakeOnTouch = false;
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
                awake = wakeOnTouch;
                plyrMovement = collision.gameObject.GetComponent<PlayerMovement>();
            }
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
    void ResetPos()
    {
        awake = false;
        wakeOnTouch = true;
        moveToTarget = true;
        //animator.SetBool("landed", false);
        sprite.flipX = false;
        transform.position = originalPosition;
    }
    
    bool rumbling = false;
    IEnumerator Rumble() //it just flips the sprite right now.
    {
        rumbling = true;
        sprite.flipX = flipx;
        flipx = !flipx;
        yield return new WaitForSeconds((Time.time-pauseTime)/rumbleTimeMod);
        rumbling = false;
    }
    
#if UNITY_EDITOR
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
#endif
}
