using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasPivoted : MonoBehaviour
{
	public bool hasPivoted = false;
	public GameObject currentPivot;
	
	// Case 1: Grappling/clicking on the pivot itself, not the general area.
	void OnMouseDown() 
	{
		hasPivoted = true;
	}
	
	// Case 2: If grappling the general area and/or the pivot itself.
	// Called by GrapplingGun.cs
	public void PivotRange()
	{
		hasPivoted = true;
	}
	
	//Check boolean hasPivoted.
	public bool HasThisPivoted()
	{
		return hasPivoted;
	}
}
