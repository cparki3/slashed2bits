using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class videoButton : MonoBehaviour {

	public bool canRestart = false;
	private levelManagerScript levelManager;
	public string loseScene;
	private GameObject sceneInfo;
	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ();	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowAd()
	{
		if (!canRestart) {
			if (Advertisement.IsReady ()) {
				Advertisement.Show ();
			}
		} else {
			if (Advertisement.IsReady("rewardedVideo"))
			{
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show("rewardedVideo", options);
			}
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			//
			Debug.Log ("BRING BACK TO LIFE!");
			sceneInfo = GameObject.Find ("SCENE_DATA");
			levelManager.revivePlayer ();
			SceneManager.UnloadSceneAsync (SceneManager.GetSceneByName (loseScene));
			sceneData sceneScript = this.sceneInfo.GetComponent <sceneData> ();
			SceneManager.MoveGameObjectToScene (sceneInfo, SceneManager.GetSceneByBuildIndex (sceneScript.sceneIndex));

			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
				break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}
	
