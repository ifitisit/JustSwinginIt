using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGrappleFlash : MonoBehaviour
{
	float flashTime = .25f;
    float flashTimer = 0.0f;
	Color originalColorGrapple;
	Color originalColorGrappleRadius;
	Color originalColorMouseClick;
	Color originalColorPickUpHand;
	[SerializeField] GameObject grapple;
	[SerializeField] GameObject grappleRadius;
	[SerializeField] GameObject mouseClick;
	[SerializeField] GameObject pickUpHand;
	[SerializeField] GameObject coin;
	bool hasGrappled = false;
	bool hasPickedUpCoin = false;
	int grCount = 0;
	bool visibleClick = true;
	bool visibleHand = true;
	
    void Start()
    {
        //originalColorGrapple = grapple.GetComponent<SpriteRenderer>().material.color;
		//Invoke("FlashColor", flashTime);
		//originalColorGrappleRadius = grappleRadius.GetComponent<SpriteRenderer>().material.color;
		//Invoke("FlashOpaque", flashTime);
		originalColorMouseClick = mouseClick.GetComponent<SpriteRenderer>().material.color;
		originalColorPickUpHand = pickUpHand.GetComponent<SpriteRenderer>().material.color;
    }

	void FlashColor(){
		grapple.GetComponent<SpriteRenderer>().material.color = new Color(0,0,1,1);
		Invoke("FlashWhite", flashTime);
	}
	
	void FlashWhite(){
		grapple.GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,1);
		if(hasGrappled == false){ 
			//Player hasn't clicked on grapple yet.
			Invoke("FlashColor", flashTime);
		}
	}
	
	void FlashColorBlue(){
		grCount++;
		grappleRadius.GetComponent<SpriteRenderer>().material.color = new Color(0,1,1,1);
		Invoke("FlashOpaque", flashTime);
	}
	
	void FlashOpaque(){
		grappleRadius.GetComponent<SpriteRenderer>().material.color = new Color(0,0,0,0);
		if(grCount <= 3){
			Invoke("FlashColorBlue", flashTime);
		}
	}
	
	void Update(){
		var objScript = grapple.GetComponent<HasPivoted>();
		if(hasGrappled==false){
			if(visibleClick==true && flashTimer <= 0){
				mouseClick.GetComponent<SpriteRenderer>().material.color = originalColorMouseClick;
				visibleClick = false;
                flashTimer = flashTime;
                
			}
			else if (flashTimer <= 0){
				mouseClick.GetComponent<SpriteRenderer>().material.color = new Color(0,0,0,0);
				visibleClick = true;
                flashTimer = flashTime;
			}
            else
            {
                flashTimer -= Time.deltaTime;
            }
		}
		if(hasPickedUpCoin==false){
			if(visibleHand==true && flashTimer <= 0){
				pickUpHand.GetComponent<SpriteRenderer>().material.color = new Color(0,0,0,0);
				visibleHand = false;
                flashTimer = flashTime;
			}
			else if (flashTimer <= 0){
				pickUpHand.GetComponent<SpriteRenderer>().material.color = originalColorPickUpHand;
				visibleHand = true;
                flashTimer = flashTime;
			}
            else
            {
                flashTimer -= Time.deltaTime;
            }
		}
		if (objScript.HasThisPivoted() == true){
			hasGrappled = true;
			mouseClick.GetComponent<SpriteRenderer>().material.color = new Color(0,0,0,0);
		}
		if(coin==null){
			hasPickedUpCoin = true;
			pickUpHand.GetComponent<SpriteRenderer>().material.color = new Color(0,0,0,0);
		}
	}
	
}
