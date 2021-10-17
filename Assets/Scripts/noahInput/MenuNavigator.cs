using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//sam i dont care, you can change whatever you what about this
public class MenuNavigator : MonoBehaviour
{
    //assigning the inputhandler
    private InputHandler inputHandler;
    private bool detectedController;
    Stack<GameObject> menuStack;
    
    public GameObject startingMenu;
    GameObject current;


    //Check for controller each frame.
    void Update()
    {
        if (inputHandler.isController && !detectedController)
        {
            //SetControllerFirstButton(allMenus[activeMenu]);
            SetFirstButtonAsActive(current);
            detectedController = true;
        }
        else if (inputHandler.isKeyboard)
        {
            detectedController = false;
        }

        bool back = Input.GetButtonDown("Back");
        if (back)
        {
            GoBack();
        }
    }
    //first thing that happens
    private void Awake()
    {
        menuStack = new Stack<GameObject>();
        current = startingMenu;
        if (detectedController) SetFirstButtonAsActive(current);
        inputHandler = GetComponent<InputHandler>();
    }

    public void SwitchMenu(GameObject menu, bool skipPuttingOnStack = false)
    {
        menu.SetActive(true);
        current.SetActive(false);
        if (!skipPuttingOnStack) menuStack.Push(current);
        current = menu;
        if (detectedController) SetFirstButtonAsActive(current);
    }

    public void SwitchMenu(GameObject menu)
    {
        SwitchMenu(menu, false);
    }

    private void SetFirstButtonAsActive(GameObject parent)
    {
        EventSystem.current.SetSelectedGameObject(findFirstSelectable(current));
    }

    private GameObject findFirstSelectable(GameObject parent)
    {
        return parent.GetComponentInChildren<Selectable>(true).gameObject;
    }

    public void GoBack()
    {
        if (menuStack.Count == 0) return;
        GameObject prevMenu = menuStack.Pop();
        SwitchMenu(prevMenu, true);
    }
}
