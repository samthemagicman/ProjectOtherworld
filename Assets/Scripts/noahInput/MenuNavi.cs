using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//sam i dont care, you can change whatever you what about this
public class MenuNavi : MonoBehaviour
{
    //assigning the inputhandler
    private InputHandler inputHandler;
    private GameObject[] allChildren;

    //first thing that happens
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        allChildren = new GameObject[transform.childCount];
        findMenus();
        switchMenu(0);
    }

    //getting a list of the menus
    public void findMenus()
    {
        int i = 0;

        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
    }

    public void switchMenu(int newMenu)
    {
        foreach (GameObject menu in allChildren)
        {
            menu.SetActive(false);
        }
        allChildren[newMenu].SetActive(true);
        ControllerFirstButton(allChildren[newMenu]);
    }

    void Update()
    {

    }
    //if controller is being used, set button to selected.
    public void ControllerFirstButton(GameObject menu)
    {
        if(inputHandler.isController == true)
        {
            GameObject layout = menu.transform.Find("layoutGroup").gameObject;
            EventSystem.current.SetSelectedGameObject(layout.transform.GetChild(0).gameObject);
        } else if(inputHandler.isKeyboard == true)
        {
            
        }
    }
}
