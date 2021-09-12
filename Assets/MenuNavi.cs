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

    //whitman is here now
    public InputHandler inputHandler;
    private bool lastMenu;

    //dead variables
    public bool mainMenuActive;
    public bool optionsMenuActive;
    public bool displayMenuActive;
    public bool audioMenuActive;
    public bool advancedMenuActive;
    public GameObject[] menus;
    void Update()
    {
        //checking which menu is active
        if (mainMenu.activeInHierarchy == true)
        {
            mainMenuActive = true;
            SelectIfControler(firstMenuButton);
            
        } else
        {
            mainMenuActive = false;
        }

        if (optionsMenu.activeInHierarchy == true)
        {
            optionsMenuActive = true;
            SelectIfControler(optionsFirstButton);
        } else
        {
            optionsMenuActive = false;
        }

        if (audioMenu.activeInHierarchy == true)
        {
            audioMenuActive = true;
            SelectIfControler(audioFirstButton);
        } else
        {
            audioMenuActive = false;
        }

        if (displayMenu.activeInHierarchy == true)
        {
            displayMenuActive = true;
            SelectIfControler(displayFirstButton);
        } else
        {
            displayMenuActive = false;
        }

        if (advancedMenu.activeInHierarchy == true)
        {
            advancedMenuActive = true;
            SelectIfControler(advancedFirstButton);
        }
        else
        {
            advancedMenuActive = false;
        }
    }

    private void SelectIfControler(GameObject button)
    {
        if (inputHandler.GetInputState() == InputHandler.eInputState.Controller)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
    public void SetActiveMenu()
    {

    }
}
