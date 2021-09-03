using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionHandler : MonoBehaviour
{
    public GameObject initialPosition;
    Vector3 targetPosition;
    Camera targetCamera;
    CameraPositionTrigger targetPositionTrigger;
    GameObject player;
    public float transitionSpeed = 2;
    public float xLimitMax;
    public float xLimitMin;
    public float yLimitMax;
    public float yLimitMin;
    Transform spawn; 

    Camera lastCameraNotExited;

    void Start()
    {
        targetPosition = initialPosition.transform.position;
        Camera.main.transform.position = initialPosition.transform.position;

        //CameraPositionTrigger[] colliders = transform.GetComponentsInChildren<CameraPositionTrigger>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CameraTrigger");
        foreach (GameObject obj in objects)
        {
            CameraPositionTrigger coll = obj.GetComponent<CameraPositionTrigger>();

            coll.PlayerEntered.AddListener((plyr) => {
                SetTargetCamera(coll);
            });

            coll.PlayerExited.AddListener(plyr =>
            {
                Camera cam = coll.GetComponent<Camera>();
                if (cam == targetCamera && lastCameraNotExited != null) // Exited new camera
                {
                    SetTargetCamera(lastCameraNotExited.GetComponent<CameraPositionTrigger>());
                } else if (cam == lastCameraNotExited) //Fully exited old camera
                {
                    lastCameraNotExited = targetCamera;
                }

            });
        }
    }

    void SetTargetCamera(CameraPositionTrigger coll)
    {
        if (lastCameraNotExited == null)
        {
            lastCameraNotExited = coll.GetComponent<Camera>();
        }
        PlayerMovement plyrMvmnt = PlayerMovement.singleton;
        player = plyrMvmnt.gameObject;
        
        SetSpawn(coll.transform);
        targetPosition = coll.gameObject.transform.position;

        if (targetCamera == null)
        {
            targetCamera = coll.GetComponent<Camera>();
            Camera.main.transform.position = targetPosition;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -20);
            Camera.main.orthographicSize = targetCamera.orthographicSize;//Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 2);
        }
        Camera temp = coll.GetComponent<Camera>();

        if (plyrMvmnt && targetCamera != temp)
        {
            Vector2 diff = temp.transform.position - targetCamera.transform.position;
            if (diff.x > diff.y)
            {
                //plyrMvmnt.Fling(new Vector2(diff.normalized.x * 15, 0), 0.2f);
            }
            else if (diff.y < 0)
            {
                //plyrMvmnt.Fling(new Vector2(0, diff.normalized.y * 50), 0.2f);
            }
        }
        targetCamera = temp;
        targetPositionTrigger = coll;
    }

    void SetSpawn(Transform trans)
    {
        spawn = trans.Find("Spawn");
        if (spawn)
        {
            Player.Respawn.Handler.lastCheckpointPosition = trans.Find("Spawn").transform.position;
            Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.CheckpointPosition;
        }
        else
        {
            Player.Respawn.Handler.currentCheckpointType = Player.Respawn.CheckpointType.ClosestPlatform;
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null && Camera.main != null)
        {
            if (targetPositionTrigger.cameraType == CameraPositionTrigger.CameraType.Static)
            {
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPosition, transitionSpeed);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -20);
                Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetCamera.orthographicSize, 2f);//Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 2);
            } else
            {
                GameObject limitObj = targetPositionTrigger.dynamicObjectIndicator;
                SpriteRenderer renderer = limitObj.GetComponent<SpriteRenderer>();

                float height = 2f * Camera.main.orthographicSize;
                float width = height * Camera.main.aspect;
                xLimitMax = limitObj.transform.position.x + renderer.size.x / 2 - width / 2;
                xLimitMin = limitObj.transform.position.x - renderer.size.x / 2 + width / 2;
                yLimitMax = limitObj.transform.position.y + renderer.size.y / 2 - height / 2;
                yLimitMin = limitObj.transform.position.y - renderer.size.y / 2 + height / 2;

                Vector3 wantedPos = player.transform.position;
                wantedPos = new Vector3(Mathf.Clamp(wantedPos.x, xLimitMin, xLimitMax), Mathf.Clamp(wantedPos.y, yLimitMin, yLimitMax));
                if (Vector2.Distance(Camera.main.transform.position, wantedPos) > 5)
                {
                    Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, wantedPos, transitionSpeed);
                } else
                {
                    Camera.main.transform.position = wantedPos;
                }
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -20);
            }
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
