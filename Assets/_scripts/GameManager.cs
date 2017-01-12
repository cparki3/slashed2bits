using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Rewired;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public string winScene;
	public int roundsToWin = 5;
	public int currentRound = 1;
	public int souls = 0;
	public characterData selectedCharacter;

	public int killCount  = 0;
	public int floorCount = 0;
	public int highScore = 0;

	public int tedKills = 0;
	public int tedFloors = 0;
	public int tedScore = 0;

	public Text tedKillText;
	public Text tedFloorText;
	public Text tedScoreText;


	public string currentLevel = "TED";
	//This class with help keep all player stats throughout the different rounds. Will be reset when going back to the main menu

	public GameObject player1 = null;
	//Need an array that contains all of the levels from the level area that gets selected
	public int[] levelArray;
	public string gameScene;


	private static bool created = false;

	void Awake()
	{
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		} else {
			Destroy (this.gameObject);
		}
		Load ();

		tedKillText.text = tedKills.ToString ();
		tedFloorText.text = tedFloors.ToString ();
		tedScoreText.text = tedScore.ToString ();
	}

	void OnDestroy()
	{
		created = false;
	}

	public void startVersusMode(){
		currentRound = 1;
		Debug.Log ("START UP THE GAME");
		int playersJoined = 0;
		if (player1 != null) {
			playersJoined++;
			Debug.Log ("p1 = " + player1.name);
		}
		SceneManager.LoadScene (gameScene);
	}

	public void gameOver()
	{
		SceneManager.LoadScene (winScene);
	}

	public void Save()
	{
		Debug.Log ("Saving Data");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/s2bInfo.dat");
		s2bData data = new s2bData ();
		data.souls = this.souls;
		if (currentLevel == "TED") {
			data.tedKills = this.tedKills;
			data.tedFloors = this.tedFloors;
			data.tedScore = this.tedScore;
		}
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/s2bInfo.dat")) {
			Debug.Log ("found save file");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/s2bInfo.dat", FileMode.Open);
			s2bData data = (s2bData)bf.Deserialize (file);
			file.Close ();

			this.souls = data.souls;
			this.tedKills = data.tedKills;
			this.tedFloors = data.tedFloors;
			this.tedScore = data.tedScore;
		}
	}
		
}

[System.Serializable]
class s2bData {
	public int souls;

	public int tedKills;
	public int tedFloors;
	public int tedScore;
}
