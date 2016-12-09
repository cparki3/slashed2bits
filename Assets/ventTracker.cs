using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventTracker : MonoBehaviour {

	public GameObject[] vents;
	public bool inVents = false;
	public int currentVent = 0;
	private GameObject player;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find ("PLAYER");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrentVent(GameObject vent)
	{
		for (int i = 0; i < vents.Length; i++) {
			if (vent.GetInstanceID() == vents [i].GetInstanceID()) {
				currentVent = i;
			}
		}
	}

	public void moveForward()
	{
		if (inVents) {
			if (currentVent < vents.Length - 1) {
				vents [currentVent].SendMessage ("leaveVent");
				currentVent++;
				vents [currentVent].SendMessage ("enterVent");
				player.transform.position = vents [currentVent].transform.position;
			} else {
				vents [currentVent].SendMessage ("leaveVent");
				currentVent = 0;
				vents [currentVent].SendMessage ("enterVent");
				player.transform.position = vents [currentVent].transform.position;
			}
		}

	}

	public void moveBack()
	{
		if (inVents) {
			if (currentVent > 0) {
				vents [currentVent].SendMessage ("leaveVent");
				currentVent--;
				vents [currentVent].SendMessage ("enterVent");
				player.transform.position = vents [currentVent].transform.position;
			} else {
				vents [currentVent].SendMessage ("leaveVent");
				currentVent = vents.Length -1;
				vents [currentVent].SendMessage ("enterVent");
				player.transform.position = vents [currentVent].transform.position;
			}
		}
	}
}
