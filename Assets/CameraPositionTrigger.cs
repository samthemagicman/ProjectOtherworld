using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPositionTrigger : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<GameObject> PlayerEntered = new UnityEvent<GameObject>();
    
    public Color gizmoOutlineColor = Color.green;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerEntered.Invoke(collision.gameObject);
        }
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
