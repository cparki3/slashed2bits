using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsPanelManager : MonoBehaviour {

	public Text soulsText;
	private GameManager GM;
	// Use this for initialization
	void Start () {
		if (GameObject.Find ("GAME_MANAGER")) {
			GM = GameObject.Find ("GAME_MANAGER").GetComponent <GameManager> ();
			soulsText.text = GM.souls.ToString ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
