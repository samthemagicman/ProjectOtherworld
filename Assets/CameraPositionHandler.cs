using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionHandler : MonoBehaviour
{
    public GameObject initialPosition;
    Vector3 targetPosition;

    void Start()
    {
        targetPosition = initialPosition.transform.position;
        Camera.main.transform.position = initialPosition.transform.position;

        CameraPositionTrigger[] colliders = transform.GetComponentsInChildren<CameraPositionTrigger>();

        foreach (CameraPositionTrigger coll in colliders)
        {
            coll.PlayerEntered.AddListener(() => {
                if (coll.transform.Find("Spawn"))
                {
                    Player.Respawn.Handler.lastCheckpointPosition = coll.transform.Find("Spawn").transform.position;
                    Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.CheckpointPosition;
                } else
                {
                    Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.ClosestPlatform;
                }
                targetPosition = coll.gameObject.transform.position;
            });
        }
    }

    void Update()
    {
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 2);
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D[] colliders = transform.GetComponentsInChildren<BoxCollider2D>();

        foreach (BoxCollider2D coll in colliders)
        {
            Camera cam = coll.gameObject.GetComponent<Camera>();
            cam.depth = -1000;
            coll.size = new Vector2(cam.orthographicSize * 3.5566666f, cam.orthographicSize * 2);
            coll.offset = Vector2.zero;
        }
    }
}
