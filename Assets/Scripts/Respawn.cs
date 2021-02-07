using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    Vector2 spawnPoint;
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
        spawnPoint = player.transform.position;
    }

    void OnPlayerDied()
    {
        player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
    }
}
