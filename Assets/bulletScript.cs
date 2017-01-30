using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Destroy (this.gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "wall") {
			coll.gameObject.SendMessage ("die", null, SendMessageOptions.DontRequireReceiver);
			Destroy (this.gameObject);
		}
		//Invoke ("killBullet", .05f);
	}



	public void killBullet()
	{
		Destroy (this.gameObject);
	}
}
