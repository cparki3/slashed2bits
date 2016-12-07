using UnityEngine;
using System.Collections;

public class cameraAdjusterY : MonoBehaviour {

	public GameObject leftButton;
	public float offset = 10f;
	private Camera mainCam;
	private Vector3 relativePosition;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		relativePosition = mainCam.transform.InverseTransformDirection (leftButton.transform.position - mainCam.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log (relativePosition.y);
		Debug.Log ("THIS POSITION " + this.transform.position.y);
		float calcPosition = (offset - relativePosition.y);
		this.transform.position = new Vector2 (this.transform.position.x, calcPosition);
	}
}
