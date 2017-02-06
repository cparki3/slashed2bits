using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class floorGeneratorScript : MonoBehaviour {

	[Header("Possible Floors")]
	public GameObject[] floors;
	public List<GameObject> createdFloors;
	public bool copsAllowed = false;
	private int floorsCreated = 0;
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
		createdFloors.Add (newFloor);
		floorsCreated++;
		if (floorsCreated > 6) {
			copsAllowed = true;
		}
		if (createdFloors.Count >= 14) {
			Destroy (createdFloors[0]);
			createdFloors.RemoveAt (0);
		}
	}

	public void getRandomFloor()
	{
		int randomFloor = Random.Range (0, floors.Length);
		floorData fdCurrent = floors [currentFloor].GetComponent <floorData> ();
		floorData fd = floors [randomFloor].GetComponent <floorData> ();
		if (fd.hasCop && !copsAllowed) {
			Debug.Log ("Floor Number: " + floorsCreated + " has a cop on it");
			getRandomFloor ();
			return;
		}
		string currentLadder = fdCurrent.ladderPosition;
		string randomLadder = fd.ladderPosition;
		if (randomFloor == currentFloor) {
			getRandomFloor ();
		} else {
			if (!copsAllowed && fd.hasCop) {
				getRandomFloor ();
				return;
			}
			if (currentLadder != randomLadder) {
				//Debug.Log (fd.hasCop);
				currentFloor = randomFloor;
			} else {
				getRandomFloor ();
			}
		}
	}
}
