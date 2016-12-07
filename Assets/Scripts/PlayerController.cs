using UnityEngine;
using System.Collections;
//using Rewired;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public int playerId = 1; // The Rewired player id of this character
	//private Player player; // The Rewired Player
	private CharacterController cc;

	private bool attack = false;

	void Awake() {
		// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
		//player = ReInput.players.GetPlayer(playerId);

		// Get the character controller
		//cc = GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		GetInput();
		ProcessInput();
	}

	private void GetInput() {
		// Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
		// whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.
		//attack = player.GetButtonDown("Attack");
	}

	private void ProcessInput() {
		// Process fire
		if(attack) {
			Debug.Log ("Player " + playerId + " is attacking");
		}
	}
}
	