using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Launches player when they swap inside of a block and jump at the same time
/// </summary>
[RequireComponent(typeof(DimensionFilter))]
public class DBooster : MonoBehaviour
{
    
    [SerializeField]
    private static readonly float FlingTime = .1f;
    [SerializeField]
    private static readonly float FlingPow = 50f;
    private static readonly float triggerSize = .9f;
    private BoxCollider2D triggerArea;
    private BoxCollider2D myCollider;
    private bool jumpPressed;
    private float jumpTime;
    private static readonly float jumpWindow = .3f;
    // Start is called before the first frame update
    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        triggerArea = gameObject.AddComponent<BoxCollider2D>();
        triggerArea.isTrigger = true;
        triggerArea.autoTiling = true;
        triggerArea.size = new Vector2(triggerSize, triggerSize);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            jumpTime = Time.time;
        }
        if(Time.time - jumpTime > jumpWindow)
        {
            jumpPressed = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && myCollider.isActiveAndEnabled && jumpPressed)
        {
            Debug.Log("Dboost Fired");
            Debug.Log(Time.time-jumpTime);
            Vector2 vel = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<PlayerMovement>().Fling(vel * FlingPow, FlingTime);
        }
    }
}
