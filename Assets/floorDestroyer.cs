using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnTriggerEnter2D( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		//Debug.Log (tagName);
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.tag );
		switch (tagName) {
		case "floor_killer":
			Destroy (col.gameObject);
			break;
		}
	}
}
