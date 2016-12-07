using UnityEngine;
using System.Collections;

public class blinkScript : MonoBehaviour {

	private SpriteRenderer eyelids;
	private int waitTime = 0;
	// Use this for initialization
	void Awake () {
		eyelids = this.GetComponent <SpriteRenderer> ();
		waitTime = Random.Range (1, 4);
		Invoke ("blink", waitTime);
	}

	void blink()
	{
		eyelids.enabled = true;
		Invoke ("openEyes", .2f);
	}

	void openEyes()
	{
		eyelids.enabled = false;
		waitTime = Random.Range (1, 4);
		Invoke ("blink", waitTime);
	}

	void OnDestroy()
	{
		CancelInvoke ();
	}

	void OnDisable()
	{
		CancelInvoke ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
