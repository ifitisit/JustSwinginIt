using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DeathPosition : MonoBehaviour
{
    public BoxCollider2D deathCollider;
    public BoxCollider2D win;
   private void OnTriggerEnter2D(Collider2D collision){

      if (collision == deathCollider || collision == win){

          int deathSegment = Mathf.RoundToInt(transform.position.x/20f);
            //7. NUMBER OF DEATHS AT EACH SEGMENT
          AnalyticsResult deathSegmentAnalytics = Analytics.CustomEvent(
              "7. Segment of death",
              new Dictionary<string,object>{
                  {"Segment", deathSegment}
              }
          );
        UnityEngine.Debug.Log("7. Death segment event log: "+ deathSegmentAnalytics);
        UnityEngine.Debug.Log("Death segment value: "+ deathSegment);

        }
        

   }
}
