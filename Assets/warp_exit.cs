using UnityEngine;
using System.Collections;
using System;
using Com.LuisPedroFonseca.ProCamera2D;

public class warp_exit : MonoBehaviour {

	private levelManagerScript levelScript;
	public Animator portalAnimator;
	public ProCamera2DCinematics cinematic;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
		cinematic.Play ();
	}
		

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			Debug.Log ("TIME TO GET THE HELL OUT OF HERE!");
			col.gameObject.SendMessage ("stopMovement", null, SendMessageOptions.DontRequireReceiver);
			col.gameObject.SendMessage ("exitPortal", null, SendMessageOptions.DontRequireReceiver);
			LeanTween.moveX (col.gameObject, this.transform.position.x, .2f);
			Invoke ("activate", 1f);
		}
	}

	void Awake () {
		if (GameObject.Find ("LEVEL_MANAGER")) {
			levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent<levelManagerScript> ();
		}
	}

	public void activate()
	{
		portalAnimator.SetTrigger ("leave");
		//SceneManager.LoadScen
		if (levelScript != null) {
			levelScript.winGame ();
		}
	}
}
