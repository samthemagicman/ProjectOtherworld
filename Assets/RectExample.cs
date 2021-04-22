using UnityEngine;
using System.Collections;
using UnityEditor;

public class RectExample : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(1, 1);
    public Rect _MyRect;
    public Rect MyRect
    {
        set
        {
            value.width = Mathf.Clamp(value.width, 1, Mathf.Infinity);
            value.height = Mathf.Clamp(value.height, 1, Mathf.Infinity);
            transform.position = value.position;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.size = value.size;
            }
            _MyRect = value;
        }
        get
        {
            return _MyRect;
        }
    }
}