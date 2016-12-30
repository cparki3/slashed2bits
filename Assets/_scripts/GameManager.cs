using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Rewired;

public class GameManager : MonoBehaviour {

	public string winScene;
	public int roundsToWin = 5;
	public int currentRound = 1;
	public int souls = 0;
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
		}
	}
		
}

[System.Serializable]
class s2bData {
	public int souls;
}
