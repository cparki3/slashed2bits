using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyPartManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Invoke ("startFade", 1f);
	}

	public void startFade()
	{
		LeanTween.alpha (this.gameObject, 0, 3f).setOnComplete(removePart);
	}

	public void removePart()
	{
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
