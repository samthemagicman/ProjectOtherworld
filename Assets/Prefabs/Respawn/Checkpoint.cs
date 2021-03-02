using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Respawn
{
    public class Checkpoint : MonoBehaviour
    {
        public LayerMask ignoreLayers;
        Vector2 respawnPosition;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Player.Respawn.Handler.respawnPosition = respawnPosition;
            }
        }

        void Start()
        {
            GetComponent<Renderer>().enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, ~ignoreLayers, -Mathf.Infinity, Mathf.Infinity);
            respawnPosition = hit.point;
        }
    }

}