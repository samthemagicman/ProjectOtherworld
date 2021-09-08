using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static void playGame()
    {
        SceneManager.LoadScene("level1_Wasteland2");
    }
    public static void quitGame()
    {
        Application.Quit();
    }
}
