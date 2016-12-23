using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKiller : MonoBehaviour {


	public float killSpeed = .2f;
	private GameObject player;
	// Use this for initialization
	void Awake () {
		player = GameObject.Find ("PLAYER");
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate ((Vector2.up * Time.deltaTime) * killSpeed, Space.World);
		if ((player.transform.position.y - this.transform.position.y) > 25) {
			this.transform.position = new Vector2 (this.transform.position.x, (player.transform.position.y - 24f));
		}
	}
}
