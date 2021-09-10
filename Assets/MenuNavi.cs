using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//sam i dont care, you can change whatever you what about this
public class MenuNavi : MonoBehaviour
{
    //yes this is all hard coded sue me you bitch

    //declaring the different menus
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject audioMenu;
    public GameObject displayMenu;
    public GameObject advancedMenu;

    //holder for the different buttons
    public GameObject firstMenuButton;
    public GameObject optionsFirstButton;
    public GameObject audioFirstButton;
    public GameObject displayFirstButton;
    public GameObject advancedFirstButton;

    //dead variables
    public bool mainMenuActive;
    public bool optionsMenuActive;
    public bool displayMenuActive;
    public bool audioMenuActive;
    public bool advancedMenuActive;

    void Update()
    {
        //checking which menu is active
        if (mainMenu.activeInHierarchy == true)
        {
            mainMenuActive = true;
        } else
        {
            mainMenuActive = false;
        }

        if (optionsMenu.activeInHierarchy == true)
        {
            optionsMenuActive = true;
        } else
        {
            optionsMenuActive = false;
        }

        if (audioMenu.activeInHierarchy == true)
        {
            audioMenuActive = true;
        } else
        {
            audioMenuActive = false;
        }

        if (displayMenu.activeInHierarchy == true)
        {
            displayMenuActive = true;
        } else
        {
            displayMenuActive = false;
        }

        if (advancedMenu.activeInHierarchy == true)
        {
            advancedMenuActive = true;
        }
        else
        {
            advancedMenuActive = false;
        }
    }
}
