using UnityEngine;
using System.Collections;

public class lightScript : MonoBehaviour {

	// Use this for initialization
	public GameObject[] onParts;
	public GameObject[] offParts;
	public GameObject fuseBox;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setScript(GameObject fB)
	{
		fuseBox = fB;
	}
	
	public void turnOn()
	{
		for(int i = 0; i < onParts.Length; i++)
		{
			onParts[i].SetActive(true);
		}
		for(int i = 0; i < offParts.Length; i++)
		{
			offParts[i].SetActive(false);
		}
		Debug.Log ("turn the light on");
	}
	
	public void turnOff()
	{
		for(int i = 0; i < onParts.Length; i++)
		{
			onParts[i].SetActive(false);
		}
		for(int i = 0; i < offParts.Length; i++)
		{
			offParts[i].SetActive(true);
		}
		Debug.Log ("turn the light off");
	}
}
