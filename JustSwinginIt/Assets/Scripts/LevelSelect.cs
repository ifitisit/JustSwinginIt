using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadScene("Level 0");
    }

    public void level2()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void level3()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
