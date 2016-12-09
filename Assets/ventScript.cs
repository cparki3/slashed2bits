using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventScript : MonoBehaviour {

	private bool hiding = false;
	private DemoScene playerScript;
	public GameObject hidingLocker;
	private ventTracker ventManager;
	private highLightToggle hlToggle;
	// Use this for initialization
	void Awake ()
	{
		hlToggle = this.GetComponent <highLightToggle> ();
		playerScript = GameObject.Find ("PLAYER").GetComponent<DemoScene> ();
		ventManager = this.GetComponentInParent <ventTracker> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void activate()
	{

		if (!hiding) {
			hiding = true;
			ventManager.inVents = true;
			ventManager.setCurrentVent (this.gameObject);
			hide ();

		} else {
			ventManager.inVents = false;
			hiding = false;
			show ();
		}
	}

	public void hide()
	{
		playerScript.hide();
		hidingLocker.SetActive (true);
	}

	public void leaveVent()
	{
		hlToggle.hideHighlight ();
		hidingLocker.SetActive (false);
	}

	public void enterVent()
	{
		hlToggle.showHighlight ();
		hidingLocker.SetActive (true);
	}

	public void show()
	{
		ventManager.vents [ventManager.currentVent].SendMessage ("leaveVent");
		playerScript.show();
		hidingLocker.SetActive (false);
	}
}
