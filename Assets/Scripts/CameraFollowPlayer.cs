using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;

    public float smoothTime = 0f;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > 30)
            {

                transform.position = player.transform.position + new Vector3(0, 0, -15 - player.transform.position.y);
            }
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, -15);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
