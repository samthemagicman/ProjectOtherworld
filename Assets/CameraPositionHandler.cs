using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionHandler : MonoBehaviour
{
    public GameObject initialPosition;
    Vector3 targetPosition;
    Camera targetCamera;

    void Start()
    {
        targetPosition = initialPosition.transform.position;
        Camera.main.transform.position = initialPosition.transform.position;

        //CameraPositionTrigger[] colliders = transform.GetComponentsInChildren<CameraPositionTrigger>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CameraTrigger");
        foreach (GameObject obj in objects)
        {
            CameraPositionTrigger coll = obj.GetComponent<CameraPositionTrigger>();

            coll.PlayerEntered.AddListener(() => {
                if (coll.transform.Find("Spawn"))
                {
                    Player.Respawn.Handler.lastCheckpointPosition = coll.transform.Find("Spawn").transform.position;
                    Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.CheckpointPosition;
                }
                else
                {
                    Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.ClosestPlatform;
                }
                targetPosition = coll.gameObject.transform.position;
                targetCamera = coll.GetComponent<Camera>();
            });
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 0.7f);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -20);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetCamera.orthographicSize, 0.04f);//Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 2);
        }
    }

    private void OnDrawGizmos()
    {
        //BoxCollider2D[] colliders = transform.GetComponentsInChildren<BoxCollider2D>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CameraTrigger");

        foreach(GameObject obj in objects)
        {
            BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
            if (coll == null)
            {
                Debug.LogWarning(obj.name + " does not have a BoxCollider on it");
            } else
            {
                Camera cam = coll.gameObject.GetComponent<Camera>();
                cam.depth = -1000;
                coll.size = new Vector2(cam.orthographicSize * 3.5566666f, cam.orthographicSize * 2);
                coll.offset = Vector2.zero;
            }
        }
    }
}
