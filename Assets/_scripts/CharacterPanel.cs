using UnityEngine;
using System.Collections;

public class CharacterPanel : MonoBehaviour {

	public GameObject readyScreen;

	public int playersReady = 0;
	public int playersJoined = 0;

	public bool ready = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (playersJoined != 0 && playersReady == playersJoined) {
			showReady ();
		} else if (playersJoined == 0 || playersReady != playersJoined) {
			hideReady ();
		}
	}
		

	private void showReady()
	{
		if (ready == false) {
			ready = true;
			readyScreen.SetActive (true);
		}
	
	}

	private void hideReady()
	{
		if (ready) {
			readyScreen.SetActive (false);
			ready = false;
		}
	}
}
