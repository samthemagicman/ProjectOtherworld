using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//sam i dont care, you can change whatever you what about this
public class MenuNavi : MonoBehaviour
{
    //assigning the inputhandler
    private InputHandler inputHandler;
    private GameObject[] allMenus;
    private int activeMenu = 0;
    private bool detectedController;
    //first thing that happens
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        allMenus = new GameObject[transform.childCount];
        FindMenus();
    }

    //getting a list of the menus 
    //main menu = index 0 ect.
    public void FindMenus()
    {
        int i = 0;

        foreach (Transform child in transform)
        {
            allMenus[i] = child.gameObject;
            i += 1;
        }
    }

    public void SwitchMenu(int newMenu)
    {
        allMenus[activeMenu].SetActive(false);
        allMenus[newMenu].SetActive(true);
        SetControllerFirstButton(allMenus[newMenu]);
        activeMenu = newMenu;
    }
    
    //Check for controller each frame.
    void Update()
    {
        if(inputHandler.isController && !detectedController)
        {
            SetControllerFirstButton(allMenus[activeMenu]);
            detectedController = true;
        }
        else if (inputHandler.isKeyboard)
        {
            detectedController = false;
        }
    }

    //if controller is being used, set button to selected.
    public void SetControllerFirstButton(GameObject menu)
    {
        if(inputHandler.isController == true)
        {
            GameObject layout = menu.transform.Find("layoutGroup").gameObject;
            GameObject firstItem = layout.transform.GetChild(0).gameObject;
            if (firstItem.GetComponent<Button>())
            {
                EventSystem.current.SetSelectedGameObject(firstItem);
            }
            else
            {
                //need something here to select non button options.
                firstItem = FindFirstItem(firstItem);
                firstItem = FindFirstSelectable(firstItem);
                EventSystem.current.SetSelectedGameObject(firstItem);
            }
        } 
    }

    // these functions are a dumb way to do it maybe?
    private GameObject FindFirstItem(GameObject firstItem)
    {
        if (firstItem.GetComponent<HorizontalOrVerticalLayoutGroup>())
        {
            firstItem = firstItem.transform.GetChild(0).gameObject;
            return FindFirstItem(firstItem);
        }
        else
        {
            return firstItem;
        }
    }
    private GameObject FindFirstSelectable(GameObject firstItem)
    {
        foreach (Transform item in firstItem.transform.parent)
        {
            if (item.GetComponent<Slider>() || item.GetComponent<Slider>())
            {
                //Debug.Log(item);   
                return item.gameObject;
            }
        }
        return (firstItem);
    }
}
