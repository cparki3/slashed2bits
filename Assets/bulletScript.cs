using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Destroy (this.gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "wall") {
			Invoke ("killBullet", .1f);
		}
		Invoke ("killBullet", .5f);
	}

	public void killBullet()
	{
		Destroy (this.gameObject);
	}
}
