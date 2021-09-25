using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSwitch : MonoBehaviour
{
    public InputHandler inputHandler;
    public GameObject keyboardBack;
    public GameObject curMenu;
    public GameObject prevMenu;
    public GameObject controllerBack;

    
    void Update()
    {
        if (inputHandler.isController == true)
        {
            bool back = Input.GetButton("Back");
            if (back)
            {
                curMenu.SetActive(false);
                prevMenu.SetActive(true);
            }
            keyboardBack.SetActive(false);
            controllerBack.SetActive(true);

        }
        else if (inputHandler.isKeyboard == true)
        {
            keyboardBack.SetActive(true);
            controllerBack.SetActive(false);
        }
    }
}
