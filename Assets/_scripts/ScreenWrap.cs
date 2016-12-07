using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

	//private FollowCamera cameraScript;
	// Use this for initialization
	void Start () {
		//cameraScript = Camera.main.GetComponent<FollowCamera> ();
	}
	
	void Update(){
		if (Camera.main.orthographicSize > 5.9) {
			//get the screen position
			Vector3 scrPos = Camera.main.WorldToScreenPoint (transform.position);
			//Check if we are too far left
			if (scrPos.x < -60)
				TeleportRight (scrPos);
			//check if we are too far right
			if (scrPos.x > Screen.width + 60)
				TeleportLeft (scrPos);
		}
	}
	
	//TeleportRight moves character from it's current x position to x position 10 pixels left from the right edge of the screen   
	void TeleportRight(Vector3 scrPos){
		
		//this is the position on the screen we want to move the character too, we only want to change it's x-coordinate
		Vector3 goalScrPos = new Vector3(Screen.width + 55, scrPos.y, scrPos.z);
		
		//translate goal screen position to world position
		Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(goalScrPos);
		
		//move player
		transform.position = targetWorldPos;
		
	}

	//TeleportLeft moves character from it's current x position to x position 10 pixels right from the left edge of the screen   
	void TeleportLeft(Vector3 scrPos){
		
		//this is the position on the screen we want to move the character too, we only want to change it's x-coordinate
		Vector3 goalScrPos = new Vector3(-55, scrPos.y, scrPos.z);
		
		//translate goal screen position to world position
		Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(goalScrPos);
		
		//move player
		transform.position = targetWorldPos;
		
	}
}


