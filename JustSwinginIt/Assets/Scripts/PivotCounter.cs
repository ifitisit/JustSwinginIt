using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class PivotCounter : MonoBehaviour
{ 
	public LevelCompleteActions levelEndpoint;
	public GameOverActions gameOver;
	public int levelNumber = 0; //x
	private double numPivotObjects = 0.0; //y
	private int numZipObjects = 0;
	
	public void PivotCounterBegin()
	{
		//1. Collect all Grappable Pivots.
		int layerNum = 9; // Potential XXX 9 for Grappable.
		List<GameObject> goa = new List<GameObject>();
		List<GameObject> zip = new List<GameObject>();
		var allObjects = FindObjectsOfType<GameObject>();
		for (int i=0;i<allObjects.Length;i++)
		{
			if(allObjects[i].layer == layerNum)
			{
				goa.Add(allObjects[i]);
			}
			else if(allObjects[i].layer == 8) {
				zip.Add(allObjects[i]);
			}
		}
		var gameObjectsAnchors = goa.ToArray();
		var zipAnchors = zip.ToArray();
		numZipObjects = zipAnchors.Length;
		numPivotObjects = gameObjectsAnchors.Length;
		
		//2. Check for all pivots whether used or not.
		double pivotsUsed = 0.0;
		int zipsUsed = 0;
		//2a. For i pivot.
		for (int i=0;i<numPivotObjects;i++)
		{
			GameObject go = gameObjectsAnchors[i];//GameObject.Find("Target");
			var objScript = (HasPivoted)go.GetComponent(typeof(HasPivoted));
			//2b. Check if pivot was used-if so, increase pivot count.
			if (objScript.HasThisPivoted() == true)
			{
				pivotsUsed++;
			}
		}

		for (int i=0;i<numZipObjects;i++)
		{
			GameObject zo = zipAnchors[i];//GameObject.Find("Target");
			var zipScript = (HasPivoted)zo.GetComponent(typeof(HasPivoted));
			//2b. Check if pivot was used-if so, increase pivot count.
			if (zipScript.HasThisPivoted() == true)
			{
				zipsUsed++;
			}
		}
		
		//2c. Calculate percentage of pivots used. Round to 3 digits.
		double percentPivotsUsed = ((pivotsUsed/numPivotObjects)*100.0);
		percentPivotsUsed = Math.Round(percentPivotsUsed, 3);
		TriggerTestAnalyticsEvent(percentPivotsUsed, pivotsUsed, zipsUsed);

	}
	//3. Analytics: Transfer fraction/percentage of zip points/grapple points used on each level.
	public void TriggerTestAnalyticsEvent(double percentPivotsUsed, double pivotsUsed, int zipsUsed)
	{
		//3a. Calculate X = Level Number
		if(levelEndpoint.levelOver == true || gameOver.levelOver == true)
		{
			levelNumber++; //XXX Should keep levelNumber in GameManager not here.
		}		
		
		//3b. Calculate Y = percentPivotsUsed
		// Debug.Log("Level Number analytics: "+ levelNumber);
		Debug.Log("Percentage of Pivots used value log: "+pivotsUsed+"/"+numPivotObjects+" "+percentPivotsUsed+"%Pivots used");
		Debug.Log("Number of zip points used value log: "+ zipsUsed);
		//3c. Generate Analytics. Responds "Ok".

		//3. SEND PERCENTAGE OF PIVOTS USED FOR EACH LEVEL, LEVEL 1 ONLY FOR NOW
		AnalyticsResult percentagePivotsAnalytics = Analytics.CustomEvent("3. Percentage of pivots used", new Dictionary<string, object>
            {
				{"Level number", 1},
                {"% Pivots Used", percentPivotsUsed }
				
            });
		Debug.Log("3. Percentage pivots used event log: "+percentagePivotsAnalytics);

		AnalyticsResult zipPointsAnalytics = Analytics.CustomEvent("4. Number of zip points used", new Dictionary<string, object>
            {
				{"Level number", 1},
                {"Number of Zips Used", zipsUsed }
				
            });
		Debug.Log("4. Zip points event log: "+zipPointsAnalytics);
		Debug.Log("Zip points value log: "+zipsUsed);
		// Debug.Log("Pivot Count Log: "+analytics_result);		
	}
}
