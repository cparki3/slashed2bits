using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Invoke ("assignTag", .2f);
	}

	private void assignTag()
	{
		this.gameObject.tag = "soul";
	}

	// Update is called once per frame
	void Update () {
		
	}
}
