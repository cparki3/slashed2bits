using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulParticleController : MonoBehaviour {

	private ParticleSystem particleS;
	private levelManagerScript levelScript;
	private ParticleSystem.Burst partBurst;
	// Use this for initialization
	void Awake() {
		particleS = this.GetComponent <ParticleSystem> ();
		if(GameObject.Find ("LEVEL_MANAGER"))
		{
			levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ();
			for (int i = 0; i < levelScript.stealthMultiplier; i++) {
				levelScript.soulCollected ();
			}
			particleS.Emit (levelScript.stealthMultiplier);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
