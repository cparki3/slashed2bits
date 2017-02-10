using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class floorGeneratorScript : MonoBehaviour {

	[Header("Possible Floors")]
	public GameObject[] floors;
	public GameObject[] level_1_floors;
	public GameObject[] level_2_floors;
	public GameObject[] level_3_floors;
	public int difficulty = 1;
	public List<GameObject> createdFloors;
	private int floorsCreated = 0;
	// Use this for initialization

	public float floorHeight = .5f;

	private int currentFloor = 0;
	private GameObject player;
	void Awake () {
		player = GameObject.Find ("PLAYER");
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.position.y - player.transform.position.y <= 14) {
			createNewFloor ();
		}
	}

	public void createNewFloor()
	{
		switch (difficulty) {
		case 1:
			floors = level_1_floors;
			break;
		case 2:
			floors = level_1_floors.Concat (level_2_floors).ToArray ();
			break;
		case 3:
			floors = level_1_floors.Concat (level_2_floors).ToArray ().Concat (level_3_floors).ToArray ();
			break;
		default:
			return;
		}
		getRandomFloor ();
		GameObject newFloor = Instantiate (floors [currentFloor], this.transform.position, Quaternion.identity) as GameObject;
		this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + floorHeight);
		createdFloors.Add (newFloor);
		floorsCreated++;
		if (floorsCreated > 10) {
			difficulty = 2;
		}
		if (createdFloors.Count >= 14) {
			Destroy (createdFloors[0]);
			createdFloors.RemoveAt (0);
		}
	}

	public void getRandomFloor()
	{
		bool nextFloorFound = false;
		Debug.Log (floors.Length);
		while(!nextFloorFound)
		{
			if (floorsCreated == 0) {
				currentFloor = Random.Range (0, floors.Length);
				nextFloorFound = true;
			} else {
				floorData currentFloorData = floors [currentFloor].GetComponent<floorData> ();
				int nextFloor = Random.Range (0, floors.Length);
				if (nextFloor == currentFloor) {
					//FLOOR IS THE SAME WHICH WE DONT WANT TO RELOOP
					//Debug.Log ("same floor found... retrying");
				} else {
					floorData nextFloorData = floors [nextFloor].GetComponent <floorData> ();
					for (int i = 0; i < currentFloorData.exitPoints.Length; i++) {
						if (nextFloorData.entryPoints.Contains (currentFloorData.exitPoints [i])) {
							currentFloor = nextFloor;
							nextFloorFound = true;
						} else {
							// EXIT AND ENTRANCES DONT HAVE A MATCH SO RETRY
							//Debug.Log ("entry and exits dont match!");
						}
					}
				}
			}
		}
	}
}
