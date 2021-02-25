using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerDeathHandler>().Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log(collider);
        /*if (collider.isTrigger && collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerDeathHandler>().Die();
        }*/
    }
}
