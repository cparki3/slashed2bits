using UnityEngine;
using System.Collections;
using System.Security.Policy;
using System;

public class VersusModeManager : MonoBehaviour {

	public string winScene;
	public int roundsToWin = 5;
	public int currentRound = 1;
	//This class with help keep all player stats throughout the different rounds. Will be reset when going back to the main menu

	public GameObject player1 = null;
	//Need an array that contains all of the levels from the level area that gets selected
	public int[] levelArray;
	public string gameScene;


	private static bool created = false;
	
	void Awake()
	{
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		} else {
			Destroy (this.gameObject);
		}
		
	}

	void OnDestroy()
	{
		created = false;
	}

	public void startVersusMode(){
		currentRound = 1;
		Debug.Log ("START UP THE GAME");
		int playersJoined = 0;
		if (player1 != null) {
			playersJoined++;
			Debug.Log ("p1 = " + player1.name);
		}
		Application.LoadLevel (gameScene);
	}
		

	public void gameOver()
	{
		Application.LoadLevel (winScene);
	}

}
