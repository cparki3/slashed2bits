using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class homeButton : MonoBehaviour {

	public ScrollRect scrollArea;
	private streetViewManager streetScript;

	// Use this for initialization
	void Awake () {
		streetScript = GameObject.Find ("STREET_UI").GetComponent <streetViewManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void scrollHome()
	{
		scrollArea.horizontalNormalizedPosition = 0;
		streetScript.hideAll ();
	}
}
