using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPositionTrigger : MonoBehaviour
{
    public UnityEvent PlayerEntered = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("oonga boonga chugga chug chug");
        PlayerEntered.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
