using UnityEngine;
using System.Collections;

public class lockerScript : MonoBehaviour {

	private bool hiding = false;
	private DemoScene playerScript;
	public GameObject hidingLocker;

	// Use this for initialization
	void Awake ()
	{
		playerScript = GameObject.Find ("PLAYER").GetComponent<DemoScene> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void activate()
	{
		
		if (!hiding) {
			hiding = true;
			hide ();
			
		} else {
			hiding = false;
			show ();
		}
	}

	public void hide()
	{
		Debug.Log ("hide in the locker quick!");
		playerScript.hide();
		hidingLocker.SetActive (true);
	}

	public void show()
	{
		Debug.Log ("come out of the locker now");
		playerScript.show();
		hidingLocker.SetActive (false);
	}
}
