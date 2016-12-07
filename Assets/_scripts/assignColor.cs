using UnityEngine;
using System.Collections;

public class assignColor : MonoBehaviour {

	private SpriteRenderer spr;
	// Use this for initialization
	void Awake () {
		spr = this.GetComponent <SpriteRenderer> ();
	}

	public void updateColor(Color c)
	{
		spr.color = c;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
