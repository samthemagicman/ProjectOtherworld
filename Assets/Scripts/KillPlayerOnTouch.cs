using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    public Collider2D collider = null;

    public enum KillBlockTrigger
    {
        Collision,
        Trigger
    }

    public KillBlockTrigger KillBlockTriggerType = KillBlockTrigger.Collision;


    private void Start()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider2D>();
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (KillBlockTriggerType == KillBlockTrigger.Collision && collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerDeathHandler>().Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (KillBlockTriggerType != KillBlockTrigger.Trigger) return;
        PlayerDeathHandler death = collider.gameObject.GetComponentInParent<PlayerDeathHandler>();
        if (death != null) death.Die();
    }
}
