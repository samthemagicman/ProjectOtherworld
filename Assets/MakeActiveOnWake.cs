using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeActiveOnWake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        if (InputHandler.singleton.isController)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
