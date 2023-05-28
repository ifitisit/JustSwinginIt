using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
   public void Setup() {
       gameObject.SetActive(true);
   }

   public void onRetryButton() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
   
   public void onMainMenuButton() {
       SceneManager.LoadScene("Main Menu");
   }
   
}
