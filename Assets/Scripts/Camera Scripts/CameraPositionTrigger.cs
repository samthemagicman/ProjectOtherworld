using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class CameraPositionTrigger : MonoBehaviour
{
    public enum CameraType {
        Static,
        Dynamic
    }
    public CameraType cameraType = CameraType.Static;
    public GameObject staticObjectIndicator;

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
        if (staticObjectIndicator != null)
        {
            SpriteRenderer renderer = staticObjectIndicator.GetComponent<SpriteRenderer>();
            Gizmos.DrawWireCube(staticObjectIndicator.transform.position, new Vector2(renderer.size.x, renderer.size.y));
        }
    }

    private void OnValidate()
    {
    }
}
