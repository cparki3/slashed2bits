using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DoorController : MonoBehaviour {

	public GameObject doorOpen;
	private SpriteRenderer doorClosed;
	public BoxCollider2D wallCollider;
	public GameObject openHiglight;
	public GameObject closedHighlight;


	private bool isOpen = false;
	// Use this for initialization
	void Awake () {
		doorClosed = this.gameObject.GetComponent <SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showHighlight()
	{
		if (isOpen) {
			openHiglight.SetActive (true);
		} else {
			closedHighlight.SetActive (true);
		}
	}

	public void hideHighlight ()
	{
		if (isOpen) {
			openHiglight.SetActive (false);
		} else {
			closedHighlight.SetActive (false);
		}
	}

	public void activate()
	{
		if (isOpen) {
			//Close the door
			isOpen = false;
			doorClosed.enabled = true;
			closedHighlight.SetActive (true);
			openHiglight.SetActive (false);
			doorOpen.SetActive (false);
			wallCollider.enabled = true;
		} else {
			//Open the door
			isOpen = true;
			closedHighlight.SetActive (false);
			openHiglight.SetActive (true);
			doorClosed.enabled = false;
			doorOpen.SetActive (true);
			wallCollider.enabled = false;
		}
	}
}
