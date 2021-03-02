using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Respawn
{
    public class Handler : MonoBehaviour
    {
        public static Vector2 respawnPosition;
        public GameObject playerPrefab;
        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            respawnPosition = player.transform.position;
            player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
        }

        void OnPlayerDied()
        {
            player = Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
            player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
        }
    }
}
