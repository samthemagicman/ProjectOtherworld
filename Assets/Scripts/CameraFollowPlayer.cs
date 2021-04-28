using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    GameObject player;

    public float smoothTime = 0f;
    public Vector2 offset = new Vector2();
    public float leftBound = -1000;
    public float rightBound = 1000;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, -15) + (Vector3)offset;
            bool outOfBounds = (targetPosition.x - horzExtent < leftBound || targetPosition.x + horzExtent > rightBound);

            if (Vector2.Distance(player.transform.position, transform.position) > 30 && !outOfBounds)
            {

                transform.position = player.transform.position + new Vector3(0, 0, -15 - player.transform.position.y) + (Vector3)offset;
            }
            targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, leftBound + horzExtent, rightBound - horzExtent), targetPosition.y, targetPosition.z);
            //transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.025f, 1000f, Time.deltaTime / Time.timeScale);
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftBound, -1000000, 0), new Vector3(leftBound, 1000000, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(rightBound, -1000000, 0), new Vector3(rightBound, 1000000, 0));
    }
}
