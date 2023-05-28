using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelCompleteActions : MonoBehaviour
{
	//0a. The PivotCounter, currently an Empty GameObject.
	public bool levelOver = false;
	[SerializeField] public PivotCounter PivotCounter;
	
    public LevelCompleteScreen LevelCompleteScreen;

    public float missedGrapples;

    public int levelNumber;

    public void Start()
    {
        AnalyticsResult levelAttempt = Analytics.CustomEvent("Level Started");
        missedGrapples = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.GetComponent<Collider2D>() != null && levelOver==false)
        {
            //5.SEND LEVEL COMPLETE EVENT WHENEVER A USER WINS LEVEL, LEVEL 1 ONLY FOR NOW
            AnalyticsResult levelCompleteAnalytics = Analytics.CustomEvent("5. Level Complete", new Dictionary<string,object>
            {
                {"Level Number", 1}
            });


            levelOver = true;
            PivotCounter.PivotCounterBegin();
            UnityEngine.Debug.Log("5. Level Complete event log: " + levelCompleteAnalytics);
            LevelCompleteScreen.Setup();
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            scoreManager.Finish();
            
            if (levelNumber == 1)
            {
                PlayerPrefs.SetInt("level1Complete", 1);
                if (scoreManager.score == scoreManager.totalScore)
                    PlayerPrefs.SetInt("level1Coins", 1);
                if (scoreManager.GetTime() <= scoreManager.goalTime)
                    PlayerPrefs.SetInt("level1Time", 1);
            }

            else if (levelNumber == 2)
            {
                PlayerPrefs.SetInt("level2Complete", 1);
                if (scoreManager.score == scoreManager.totalScore)
                    PlayerPrefs.SetInt("level2Coins", 1);
                if (scoreManager.GetTime() <= scoreManager.goalTime)
                    PlayerPrefs.SetInt("level2Time", 1);
            }

            else if (levelNumber == 3)
            {
                PlayerPrefs.SetInt("level3Complete", 1);
                if (scoreManager.score == scoreManager.totalScore)
                    PlayerPrefs.SetInt("level3Coins", 1);
                if (scoreManager.GetTime() <= scoreManager.goalTime)
                    PlayerPrefs.SetInt("level3Time", 1);
            }

            PlayerPrefs.Save();

            //9. SEND NUMBER OF MISSED GRAPPLES ON EACH LEVEL, LEVEL 1 ONLY FOR NOW
            AnalyticsResult otherMissedGrappleAnalytics = Analytics.CustomEvent("9. Missed Grapples", new Dictionary<string, object>
            {
                {"Level Number", 1},
                {"Missed Grapples", missedGrapples}
                
            });
            UnityEngine.Debug.Log("Missed Grapples value log: " + missedGrapples);

            AnalyticsResult coinCountAnalytics = Analytics.CustomEvent("1. Coins collected", new Dictionary<string, object>
            {
                {"Level Number", 1},
                {"Coins collected", ScoreManager.instance.score}
            });
            UnityEngine.Debug.Log("Coin count value log :" + ScoreManager.instance.score);
            UnityEngine.Debug.Log("1. Coin count event log :" + coinCountAnalytics);


            // StartCoroutine(WaitThenReload());

            
            
        }
    }

    // IEnumerator WaitThenReload()
    // {
    //     yield return new WaitForSeconds(5);
    //     Scene scene = SceneManager.GetActiveScene();
    //     SceneManager.LoadScene(scene.name);
    // }

}
