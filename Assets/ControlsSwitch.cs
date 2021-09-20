using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSwitch : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject keyboardControls;
    public GameObject controllerControls;
    public GameObject keyboardBack;
    public GameObject controllerBack;
    
    void Update()
    {
        if(inputHandler.isController == true)
        {
            controllerControls.SetActive(true);
            controllerBack.SetActive(true);
            keyboardControls.SetActive(false);
            keyboardBack.SetActive(false);
        } else if (inputHandler.isKeyboard == true)
        {
            controllerControls.SetActive(false);
            controllerBack.SetActive(false);
            keyboardControls.SetActive(true);
            keyboardBack.SetActive(true);
        }
    }
}
