using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritSize : MonoBehaviour
{
    public GameObject sizeTemplate;
    void Awake()
    {
        GetComponent<SpriteRenderer>().size = sizeTemplate.GetComponent<SpriteRenderer>().size;
        transform.position = sizeTemplate.transform.position;
    }


}
