using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//sam i dont care, you can change whatever you what about this
public class MenuNavi : MonoBehaviour
{
    public InputHandler inputHandler;
    
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
    }
    //first, we need to get the current menu
    public void findMenus()
    {
        int i = 0;

        GameObject[] allChildren = new GameObject[transform.childCount];

        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
    }

    public void SelectIfController(GameObject button)
    {
        if(inputHandler.isController == true)
        {
            EventSystem.current.SetSelectedGameObject(button);
            print(EventSystem.current.currentSelectedGameObject);
        } else if(inputHandler.isKeyboard == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            print(EventSystem.current.currentSelectedGameObject);
        }
    }
}
