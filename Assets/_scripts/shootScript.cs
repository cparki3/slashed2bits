using UnityEngine;
using System.Collections;

public class shootScript : MonoBehaviour {

	private CopController copScript;

	public void shoot()
	{
		copScript.fireWeapon ();
	}
	// Use this for initialization
	void Awake () {
		copScript = GetComponentInParent <CopController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
