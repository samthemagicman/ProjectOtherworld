using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public float radius = 20;
    bool playerInRadius = false;
    GameObject player;
    DistanceJoint2D joint2d;
    bool swinging = false;

    void Start()
    {
        joint2d = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRadius && !swinging && Input.GetKeyDown(KeyCode.E))
        {
            swinging = true;
            player.GetComponent<PlayerMovement>().Fling(player.GetComponent<Rigidbody2D>().velocity.normalized * 15, 1);
            joint2d.connectedBody = player.GetComponent<Rigidbody2D>();
            PlayerMovement.DisableControls();

        }

        if (swinging && Input.GetKeyUp(KeyCode.E))
        {
            swinging = false;
            joint2d.connectedBody = null;
            PlayerMovement.EnableControls();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision);
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            playerInRadius = true;
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            playerInRadius = false;
        }
    }
}
