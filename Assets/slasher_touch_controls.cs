using UnityEngine;
using System.Collections;

public class slasher_touch_controls : MonoBehaviour {

	public DemoScene slasherScript;
	// Use this for initialization
	void Start () {
	
	}

	public void leftPressed()
	{
		slasherScript.moveLeft = true;
	}

	public void leftUp()
	{
		slasherScript.moveLeft = false;
	}

	public void rightPressed()
	{
		slasherScript.moveRight = true;
	}

	public void rightUp()
	{
		slasherScript.moveRight = false;
	}

	public void killPressed()
	{
		slasherScript.checkInterraction ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
