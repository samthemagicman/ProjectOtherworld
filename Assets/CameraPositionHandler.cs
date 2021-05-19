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
                targetPosition = coll.gameObject.transform.position;
            });
        }
    }

    void Update()
    {
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPosition, 2);
    }
}
