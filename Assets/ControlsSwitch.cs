using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSwitch : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject keyboardControls;
    public GameObject controllerControls;
    
    void Update()
    {
        if(inputHandler.isController == true)
        {
            controllerControls.SetActive(true);
            keyboardControls.SetActive(false);
        } else if (inputHandler.isKeyboard == true)
        {
            controllerControls.SetActive(false);
            keyboardControls.SetActive(true);
        }
    }
}
