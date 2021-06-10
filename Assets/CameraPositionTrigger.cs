using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPositionTrigger : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent PlayerEntered = new UnityEvent();
    
    public Color gizmoOutlineColor = Color.green;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerEntered.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
    }

    private void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Gizmos.color = gizmoOutlineColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
