using UnityEngine;
using System.Collections;

public class fuseBox : MonoBehaviour {

	public GameObject[] lights;
	public bool lightsOn = true;

	// Use this for initialization
	void Awake () {
		for(int i = 0; i < lights.Length; i++)
		{
			lights[i].SendMessage("setScript", this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void activate()
	{
		if(lightsOn)
		{
			lightsOn = false;
			turnOffLights();
		}
		else
		{
			lightsOn = true;
			turnOnLights();
		}
	}
	
	public void turnOnLights()
	{
		for(int i = 0; i < lights.Length; i++)
		{
			lights[i].SendMessage("turnOn");
		}
	}
	
	public void turnOffLights()
	{
		for(int i = 0; i < lights.Length; i++)
		{
			lights[i].SendMessage("turnOff");
		}
	}
}
