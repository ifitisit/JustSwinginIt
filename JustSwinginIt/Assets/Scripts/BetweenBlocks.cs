using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class BetweenBlocks : MonoBehaviour
{
    static float timeInterval;
    public CircleCollider2D player;
    public BoxCollider2D death;
    public BoxCollider2D win;
    private bool change;
    string checkpoint;
    static int platform;
    private Vector3 Location;
    static float x;
    static Dictionary<string, float> betweenValues;
    static int total;
    static bool end;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
        BetweenBlocks[] allBlocks = FindObjectsOfType<BetweenBlocks>();
        total = allBlocks.Length - 2;
        betweenValues = new Dictionary<string, float>();
        x = 0;
        platform = 0;
        timeInterval = 0;
        change = false;
    }

    private string GetPlatformTimeCategory(int platformTime){
        if (0<=platformTime && platformTime<=2){
            return "0-2s";
        }
        else if(2<=platformTime && platformTime<=5){
            return "2-5s";
        }
        else{
            return ">5s";
        }
    }


    private void OnTriggerEnter2D(Collider2D collision2)
    {
        if ((this.gameObject.GetComponent<BoxCollider2D>() == death || this.gameObject.GetComponent<BoxCollider2D>() == win) && end == false)
        {
            end = true;

            foreach (KeyValuePair<string, float> entry in betweenValues)
            {
                int platformTime = Convert.ToInt32(entry.Value);
                string platformTimeCategory = GetPlatformTimeCategory(platformTime);

                AnalyticsResult timeBetweenPlatforms = Analytics.CustomEvent("10. Time taken to reach platform " + entry.Key, new Dictionary<string, object>
                {
                    {"time", platformTimeCategory}

                });
                UnityEngine.Debug.Log("10. Time Between Each Platform event log: " + timeBetweenPlatforms);
                UnityEngine.Debug.Log("Time between each platform value log:" + entry.Key+" "+platformTimeCategory);
            }
        }
        else {
            if (collision2 == player && collision2.transform.position.x > x)
            {
                x = collision2.transform.position.x + 5;
                platform = platform + 1;
                change = true;
            }
        }

        if (collision2 == player && collision2.transform.position.x > x)
        {
            x = collision2.transform.position.x + 5;
            platform = platform + 1;
            change = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (change && platform >= 1)
        {
            betweenValues.Add(platform.ToString(), timeInterval / total);
            change = false;
            timeInterval = 0;
            //AnalyticsResult analyticsResult = Analytics.CustomEvent(
            //"Time For Each Segment",
            //new Dictionary<string, object>{
            //    {checkpoint, timeInterval}
            //});

        }
        timeInterval += Time.deltaTime;
    }
}