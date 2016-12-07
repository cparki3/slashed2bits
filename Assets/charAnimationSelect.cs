using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class charAnimationSelect : MonoBehaviour {

	public GameObject TED;
	public GameObject MASON;
	public GameObject SUGAR;
	public Text nameTxt;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showAnimation(String charName)
	{
		TED.SetActive (false);
		MASON.SetActive (false);
		SUGAR.SetActive (false);
		switch (charName) {
		case "TED":
			TED.SetActive (true);
			nameTxt.text = charName;
			break;
		case "MASON":
			MASON.SetActive (true);
			nameTxt.text = charName;
			break;
		case "SUGAR":
			SUGAR.SetActive (true);
			nameTxt.text = charName;
			break;
		default:
			Debug.Log ("no animation found for that name");
			break;
		}
	}
}
