using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class houseButton : MonoBehaviour, IPointerClickHandler {

	public GameObject levelScreen;
	public GameObject charSelect;
	public GameObject playButton;
	private streetViewManager streetScript;
	// Use this for initialization
	void Awake () {
		streetScript = GameObject.Find ("STREET_UI").GetComponent <streetViewManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		levelScreen.SetActive (true);
		//charSelect.SetActive (true);
		streetScript.playButton = this.playButton;
		playButton.SetActive (true);
		//Debug.Log ("YOU CLICKED ON MY HOUSE!");
	}
}
