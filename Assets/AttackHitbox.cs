using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackHitbox : MonoBehaviour
{
    public UnityEvent<Enemy> onEnemyHit;
    public UnityEvent<GameObject> onPlayerHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy)
        {
            onEnemyHit.Invoke(enemy);
        }

        if (collision.gameObject.tag == "Player")
        {
            onPlayerHit.Invoke(collision.gameObject);
        }
    }
}
