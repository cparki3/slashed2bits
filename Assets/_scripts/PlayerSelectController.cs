using UnityEngine;
using System.Collections;
using Rewired;
using System.Security.AccessControl;
using System;

public class PlayerSelectController : MonoBehaviour {

	[Header("Rewired")]
	public int playerId = 0;
	public bool playerConnected = false;
	private Player player;
	public CharacterSelect charSelectScript;
	public GameObject readyScreen;
	private bool canMove = false;
	public bool joined = false;
	public bool charSelected = false;

	private GameManager GM;

	private CharacterPanel charPanel;
	// Use this for initialization
	void Awake () {
		if (GameObject.Find ("GAME_MANAGER")) {
			GM = GameObject.Find ("GAME_MANAGER").GetComponent <GameManager> ();
		}
		player = ReInput.players.GetPlayer (playerId);
		charPanel = this.GetComponentInParent <CharacterPanel>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((player.controllers.joystickCount > 0) && !playerConnected) {
			playerConnected = true;
		}
		if ((player.controllers.joystickCount == 0) && playerConnected) {
			playerConnected = false;
		}

		if (!charSelected) {

			if (player.GetAxis ("Move X") > 0.5f && canMove) {
				canMove = false;
				charSelectScript.nextChild ();
			}

			if (player.GetAxis ("Move X") < -0.5f && canMove) {
				canMove = false;
				charSelectScript.prevChild ();
			}

			if (player.GetAxis ("Move X") <= 0.5 && player.GetAxis ("Move X") >= -0.5) {
				canMove = true;
			}
		}

		if (player.GetButtonDown ("Jump")) {
			selectCharacter ();
		}

		if (player.GetButtonDown ("Slide")) {
			if (charSelected) {
				resetCharacter ();
			}
		}
	}

	private void joinGame()
	{
		charPanel.playersJoined++;
	}

	private void leaveGame()
	{
		charPanel.playersJoined--;
		joined = false;
	}

	public void selectCharacter()
	{
		if (charSelected != true) {
			charSelected = true;
			charPanel.playersReady++;
			//readyScreen.SetActive (true);
			switch (playerId) {
			case 0:
				GM.player1 = charSelectScript.charInfo.charDataPrefab;
				break;
			default:
				break;
			}
			startGame ();
		} 
	}

	public void startGame()
	{
		GM.startVersusMode ();
	}

	public void resetCharacter()
	{
		charPanel.playersReady--;
		charSelected = false;
		readyScreen.SetActive (false);
		switch (playerId)
		{
		case 0:
			GM.player1 = null;
			break;
		default:
			break;
		}
	}
		


}
