using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(-10, 10)]
    public int parallaxValue;

    GameObject player;

    Vector3 playerPositionDelta;
    Vector3 lastPlayerPosition;
    void Start()
    {
        player = Camera.main.gameObject;

        transform.position = transform.position - Vector3.Scale(transform.position - player.transform.position, new Vector3((float)parallaxValue / 10, (float)parallaxValue / 10, (float)parallaxValue / 10));
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (lastPlayerPosition == null)
            {
                lastPlayerPosition = player.transform.position;
            }

            playerPositionDelta = player.transform.position - lastPlayerPosition;
            playerPositionDelta = Vector3.Scale(playerPositionDelta, new Vector3((float)parallaxValue / 10, 0, 0));

            transform.position = transform.position + playerPositionDelta;

            lastPlayerPosition = player.transform.position;
        } else
        {
            player = Camera.main.gameObject;
        }
    }
}
