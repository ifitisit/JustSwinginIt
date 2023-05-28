using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameOverActions : MonoBehaviour
{   
    //0a. The PivotCounter, currently an Empty GameObject.
	  public bool levelOver = false;
	  [SerializeField] public PivotCounter PivotCounter;
    public GameOverScreen GameOverScreen;
    [SerializeField] private LevelCompleteActions manager;
	
	[SerializeField] bool noDeathMode;
	[SerializeField] private GameObject player;
    [SerializeField] private GrapplingRope rope;
    Vector3 direction = new Vector3(0,0,0);


	void Start(){
		direction = player.transform.position;
	}
	
    private void OnTriggerEnter2D(Collider2D collision){

      if (collision.GetComponent<Collider2D>()!=null && levelOver==false && noDeathMode==false){


          //6. NUMBER OF DEATHS ON EACH LEVEL, LEVEL 1 ONLY FOR NOW
          AnalyticsResult deathResultAnalytics = Analytics.CustomEvent(
              "6. Unsuccessful attempts",
              new Dictionary<string,object>{
                  {"Level Number", 1},
              }
          );

        

		//0b. Call PivotCounter to record data.
		      levelOver = true;
		      PivotCounter.PivotCounterBegin();         

            //9. SEND NUMBER OF MISSED GRAPPLES ON EACH LEVEL, LEVEL 1 ONLY FOR NOW
            AnalyticsResult missedGrappleAnalytics = Analytics.CustomEvent("9. Missed Grapples", new Dictionary<string, object>
            {
                {"Level Number", 1},
                {"Missed Grapples", manager.missedGrapples}
        
            });
            UnityEngine.Debug.Log("9. Missed Grapples event log: " + missedGrappleAnalytics);
            UnityEngine.Debug.Log("6. Player dead event log: "+ deathResultAnalytics);


            GameOverScreen.Setup();
            FindObjectOfType<ScoreManager>().Finish();
            // StartCoroutine(WaitThenReload());
        }
		else if(collision.GetComponent<Collider2D>()!=null && levelOver==false && noDeathMode==true){
			Vector3 poseCamera = direction;
            player.transform.position = poseCamera;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            rope.enabled = false;
		}
    }

    // IEnumerator WaitThenReload()
    // {
    //     yield return new WaitForSeconds(5);
    //     Scene scene = SceneManager.GetActiveScene();
    //     SceneManager.LoadScene(scene.name);
    // }

}
