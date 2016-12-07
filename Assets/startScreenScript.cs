using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


public class startScreenScript : MonoBehaviour {

	public String charSelectScene ="";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadCharSelect()
	{
		SceneManager.LoadScene (charSelectScene);
	}
		
}
