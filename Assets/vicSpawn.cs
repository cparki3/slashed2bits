using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vicSpawn : MonoBehaviour {

	public GameObject[] victims;

	// Use this for initialization
	void Awake () {
		GameObject newVictim = victims [Random.Range (0, victims.Length - 1)];
		GameObject victim = Instantiate (newVictim, this.transform.position, Quaternion.identity);
		victim.transform.parent = this.transform.parent;
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
