using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorGeneratorScript : MonoBehaviour {

	[Header("Possible Floors")]
	public GameObject[] floors;
	// Use this for initialization

	public float floorHeight = .5f;

	private int currentFloor = 0;
	private GameObject player;
	void Awake () {
		player = GameObject.Find ("PLAYER");
		createNewFloor ();
		createNewFloor ();
		createNewFloor ();
		createNewFloor ();
		createNewFloor ();
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.position.y - player.transform.position.y <= 14) {
			createNewFloor ();
		}
	}

	public void createNewFloor()
	{
		getRandomFloor ();
		GameObject newFloor = Instantiate (floors [currentFloor], this.transform.position, Quaternion.identity) as GameObject;
		this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + floorHeight);
	}

	public void getRandomFloor()
	{
		int randomFloor = Random.Range (0, floors.Length);
		if (randomFloor == currentFloor) {
			getRandomFloor ();
		} else {
			string currentLadder = floors [currentFloor].GetComponent <floorData> ().ladderPosition;
			string randomLadder = floors [randomFloor].GetComponent <floorData> ().ladderPosition;

			if (currentLadder != randomLadder) {
				currentFloor = randomFloor;
			} else {
				getRandomFloor ();
			}
		}
	}
}
