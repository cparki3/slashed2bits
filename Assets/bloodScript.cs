using UnityEngine;
using System.Collections;

public class bloodScript : MonoBehaviour {

	public lose_animation_controller loseControl;

	// Use this for initialization
	void Awake () {
		loseControl = this.GetComponentInParent<lose_animation_controller> ();
	}

	public void animComplete()
	{
		loseControl.bloodComplete ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
