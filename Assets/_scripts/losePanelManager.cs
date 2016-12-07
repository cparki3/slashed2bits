using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class losePanelManager : MonoBehaviour {

	public string startScene;
	private sceneData sceneInfo;

	// Use this for initialization
	void Awake () {
		sceneInfo = GameObject.Find ("SCENE_DATA").GetComponent <sceneData>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void resetLevel()
	{
		SceneManager.LoadScene (sceneInfo.sceneIndex, LoadSceneMode.Single);
	}

	public void backToStart()
	{
		SceneManager.LoadScene (startScene, LoadSceneMode.Single);
	}
}
