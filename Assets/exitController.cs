using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class exitController : MonoBehaviour {

	public string exitScene;
	private levelManagerScript levelScript;
	// Use this for initialization
	void Awake () {
		if (GameObject.Find ("LEVEL_MANAGER")) {
			levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent<levelManagerScript> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate()
	{
		//SceneManager.LoadScen
		if (levelScript != null) {
			levelScript.winGame ();
		}
	}
}
