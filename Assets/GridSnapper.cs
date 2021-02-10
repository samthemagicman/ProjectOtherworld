using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSnapper : MonoBehaviour
{
    Vector3 lastPosition;
    Vector2 lastSize;
    new SpriteRenderer renderer;
    public static float gridSize = 1;
    public static float resizeGridSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        lastSize = renderer.size;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        renderer = GetComponent<SpriteRenderer>();
        
        Vector3 newPosition = transform.position;
        Vector2 newSize = renderer.size;
        Vector2 sizeDelta = newSize - lastSize;
        Vector3 positionDelta = newPosition - lastPosition;
        //transform.position = new Vector3(Mathf.Round(transform.position.x / gridSize) * gridSize, Mathf.Round((transform.position.y - (sizeDelta.y - positionDelta.y)) / gridSize) * gridSize, Mathf.Round(transform.position.z / gridSize) * gridSize);
        renderer.size = new Vector2(Mathf.Round(renderer.size.x / resizeGridSize) * resizeGridSize, Mathf.Round(renderer.size.y / resizeGridSize) * resizeGridSize);
        newSize = renderer.size;
        Vector2 sizeDeltaAfterChange = newSize - lastSize;
        if (sizeDelta.magnitude > 0)
        {
            transform.position = new Vector3(lastPosition.x + sizeDeltaAfterChange.x / 2, lastPosition.y - sizeDeltaAfterChange.y / 2, transform.position.z);
        } else if (positionDelta.magnitude > 0)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x / gridSize) * gridSize, Mathf.Round(transform.position.y / gridSize) * gridSize, Mathf.Round(transform.position.z / gridSize) * gridSize);
        }
        lastPosition = transform.position;
        lastSize = renderer.size;
    }
}
