using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
    }

    void OnPlayerDied()
    {
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player.GetComponent<PlayerDeathHandler>().onDied.AddListener(() => Invoke("OnPlayerDied", 1));
    }
}
