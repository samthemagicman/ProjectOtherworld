using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager 
{
    public static bool isPaused { get; private set; } // this makes the variable read only for other scripts, but you can change it here
    public static void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }
    public static void UnPauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}
