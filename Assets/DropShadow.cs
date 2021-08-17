using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DropShadow : MonoBehaviour
{
    public Color shadowColor = Color.black;
    public Vector2 offset = new Vector2(0.2f, 0.2f);
    public bool shadowEnabled = false;
    GameObject shadowObject = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shadowEnabled == true && shadowObject == null)
        {
            shadowObject = new GameObject("Shadow", typeof(SpriteRenderer));
            shadowObject.transform.SetParent(transform);
            shadowObject.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            shadowObject.transform.localPosition = Vector3.zero + (Vector3) offset;
            shadowObject.transform.localPosition = new Vector3(0, 0, -1) + (Vector3) offset;
            shadowObject.transform.localScale = Vector3.one;
            //shadowObject.hideFlags = HideFlags.HideInHierarchy;
        } else if (shadowEnabled == false && shadowObject != null)
        {
            DestroyImmediate(shadowObject);
        }

        shadowObject.GetComponent<SpriteRenderer>().color = shadowColor;
        shadowObject.transform.localPosition = new Vector3(0, 0, -1) + (Vector3)offset;
    }
}
