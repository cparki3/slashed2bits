using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starterVictim : MonoBehaviour {

	public GameObject partLauncher;
	public GameObject blood;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void die()
	{
		Instantiate (partLauncher, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), Quaternion.identity);
		Instantiate(blood, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), blood.transform.rotation);
		Destroy (this.gameObject);
	}
}
