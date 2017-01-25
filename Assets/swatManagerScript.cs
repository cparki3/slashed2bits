using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Permissions;

public class swatManagerScript : MonoBehaviour {

	public GameObject swatPrefab;
	public List<GameObject> swatTeam;
	public float speedBoost = 1f;
	public GameObject player;
	public float originalSpeed = 1f;
	public float walkSpeed = 1f;
	public float speedIncrease = .3f;
	// Use this for initialization
	void Awake () {
		Invoke ("spawnSwat", 10f);
		player = GameObject.Find ("PLAYER");
	}

	public void spawnSwat()
	{
		GameObject newSwat = Instantiate (swatPrefab, this.transform.position, Quaternion.identity) as GameObject;
		swatTeam.Add (newSwat);
		if (swatTeam.Count < 5) {
			Invoke ("spawnSwat", 1f);
		}
	}

	public void speedUpSwat()
	{
		/*for (int i = 0; i < swatTeam.Count; i++) {
			swatTeam [i].SendMessage ("boostSpeed", speedBoost);
		}*/
		walkSpeed = walkSpeed += .3f;
	}

	public void killAllSwat()
	{
		GameObject[] livingSwat = GameObject.FindGameObjectsWithTag ("swat");
		/*foreach(GameObject go in swatTeam)
		{
			go.SendMessage ("die", null, SendMessageOptions.DontRequireReceiver);
		}*/
		/*swatTeam.ForEach ()
		{
			
		};*/
		for (int i = 0; i < livingSwat.Length; i++) {
			livingSwat[i].SendMessage ("die", null, SendMessageOptions.DontRequireReceiver);
		}
		walkSpeed = originalSpeed;
		swatTeam.Clear ();
	}

	public void targetDead()
	{
		walkSpeed = originalSpeed;
		for (int i = 0; i < swatTeam.Count; i++) {
			swatTeam [i].SendMessage ("resetSpeed", originalSpeed, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void resetSwat()
	{
		if (player.transform.position.y - 8 > this.transform.position.y) {
			this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - 8, 0);
		}
		spawnSwat ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
