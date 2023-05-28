using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class GetAvgSpeed : MonoBehaviour
{
    [Header("Physics Ref:")]
    public Rigidbody2D m_rigidbody;
    public BoxCollider2D deathBed;
    public BoxCollider2D endPoint;

    private float averageSpeed;
    private float lastTimeSpan;
    private bool keepUpdatingAvgSpeed;
    


    // Start is called before the first frame update
    void Start()
    {
        lastTimeSpan = 0;
        averageSpeed = 0;
        keepUpdatingAvgSpeed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (keepUpdatingAvgSpeed)
        {
            updateAvgSpeed();
        }
    }

    private string GetSpeedCategory(int mySpeed){
        if (0<=mySpeed && mySpeed<=15){
            return "LOW";
        }
        else if(15<=mySpeed && mySpeed<=30){
            return "NORMAL";
        }
        else if(31<=mySpeed && mySpeed<=50){
            return "HIGH";
        }
        else{
            return "EXTREME";
        }
    }

      private string GetTimeCategory(int myTime){
        if (0<=myTime && myTime<=10){
            return "0-10s";
        }
        else if(11<=myTime && myTime<=20){
            return "11-20s";
        }
        else if(21<=myTime && myTime<=30){
            return "21-30s";
        }
        else if(31<=myTime && myTime<=50){
            return "31-50s";
        }
        else{
            return ">50s";
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>() != null && (collision == endPoint))
        {
            // Debug.Log("Collided with " + collision.ToString());
            // Debug.Log("Avg. speed: " + averageSpeed.ToString());
            keepUpdatingAvgSpeed = false;
            int intSpeed= Convert.ToInt32(averageSpeed);

            string speedCategory = GetSpeedCategory(intSpeed);

            int intTime= Convert.ToInt32(lastTimeSpan);

            string timeCategory = GetTimeCategory(intTime);

            //8. SEND AVERAGE SPEED CATEGORY ON EACH LEVEL, LEVEL 1 FOR NOW
            AnalyticsResult avgSpeedAnalytics = Analytics.CustomEvent(
            "8. Average Speed Category",
            new Dictionary<string, object>{
                {"Level Number",1},
                {"Avg Speed Category", speedCategory}
            });

            AnalyticsResult timeTakenAnalytics = Analytics.CustomEvent(
            "2. Time taken to complete level",
            new Dictionary<string, object>{
                {"Level Number",1},
                {"Time taken range", timeCategory}
            });


            UnityEngine.Debug.Log("8. Average speed event log: "+avgSpeedAnalytics);
            UnityEngine.Debug.Log("Average speed category value log: "+speedCategory);
            UnityEngine.Debug.Log("2. Time taken event log: "+timeTakenAnalytics);
            UnityEngine.Debug.Log("Time range: "+timeCategory);

        }
    }

    private void updateAvgSpeed()
    {
        averageSpeed = (averageSpeed * lastTimeSpan + m_rigidbody.velocity.magnitude * Time.deltaTime) / (lastTimeSpan + Time.deltaTime);
        lastTimeSpan += Time.deltaTime;
    }

    //void OnDestroy()
    //{
    //    Debug.Log("OnDestroy Avg. speed: " + averageSpeed.ToString());
    //}
}
