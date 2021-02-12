using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public Respawn respawner;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        respawner.spawnPoint = transform.position;
    }
}
