using UnityEngine;
using System.Collections;

public class highLightToggle : MonoBehaviour {

	public GameObject highlighted;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void showHighlight()
	{
		highlighted.SetActive(true);
	}
	
	public void hideHighlight()
	{
		highlighted.SetActive(false);
	}
}
