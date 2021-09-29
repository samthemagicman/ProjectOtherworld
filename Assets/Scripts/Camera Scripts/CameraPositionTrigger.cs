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
    public GameObject dynamicObjectIndicator;

    [HideInInspector]
    public UnityEvent<GameObject> PlayerEntered = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> PlayerExited = new UnityEvent<GameObject>();

    public Color gizmoOutlineColor = Color.green;

    void OnCollisionEnterHandle(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerEntered.Invoke(collision.gameObject);
        }
    }
    void OnCollisionExitHandle(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerExited.Invoke(collision.gameObject);
        }
    }

    private void Start()
    {
        if (cameraType == CameraType.Dynamic && dynamicObjectIndicator != null)
        {
            dynamicObjectIndicator.GetComponent<OnTriggerEvent>().OnTriggerEnter2DEvent.AddListener((Collider2D collision) =>
            {
                OnCollisionEnterHandle(collision);
            });
            dynamicObjectIndicator.GetComponent<OnTriggerEvent>().OnTriggerExit2DEvent.AddListener((Collider2D collision) =>
            {
                OnCollisionExitHandle(collision);
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cameraType == CameraType.Static)
        {
            OnCollisionEnterHandle(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cameraType == CameraType.Static)
        {
            OnCollisionExitHandle(collision);
        }
    }

    private void OnDrawGizmos()
    {
        Camera cam = GetComponent<Camera>();
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Gizmos.color = gizmoOutlineColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
        if (dynamicObjectIndicator != null)
        {
            SpriteRenderer renderer = dynamicObjectIndicator.GetComponent<SpriteRenderer>();
            Gizmos.DrawWireCube(dynamicObjectIndicator.transform.position, new Vector2(renderer.size.x, renderer.size.y));
        }
    }

    private void OnValidate()
    {
    }
}
