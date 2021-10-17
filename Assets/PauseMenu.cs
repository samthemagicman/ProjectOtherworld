using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused;
    public UnityEvent onPauseMenu = new UnityEvent();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        if (pauseMenuUI.activeSelf == true) return;
        pauseMenuUI.SetActive(true);
        GameManager.PauseGame();
        onPauseMenu.Invoke();
    }

    public void DeactivateMenu()
    {
        if (pauseMenuUI.activeSelf == false) return;
        pauseMenuUI.SetActive(false);
        GameManager.UnPauseGame();

    }
}

