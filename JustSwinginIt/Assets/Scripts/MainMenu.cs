using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void onStartButton() {
        SceneManager.LoadScene("Level Select");
    }

    public void onInstructionsButton() {
        SceneManager.LoadScene("Instructions");
    }
    
    public void onQuitButton() {
        Application.Quit();
        Debug.Log("Game quit!");
    }


}
