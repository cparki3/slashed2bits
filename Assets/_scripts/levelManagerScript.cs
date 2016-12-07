using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.ootii.Messages;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class levelManagerScript : MonoBehaviour {

	public float time;
	public Text timerLabel;
	public bool timerRunning = false;
	public GameObject timerContainer;
	public string loseScene;
	public string winScene;
	public GameObject cop;
	private GameObject sceneData;
	public GameObject exit;
	public List<GameObject> victimList;
	// Use this for initialization
	void Awake () {
		MessageDispatcher.AddListener ("SEND_COPS", sendCops);
		sceneData = GameObject.Find ("SCENE_DATA");
		GameObject[] victims = GameObject.FindGameObjectsWithTag ("victim");
		for (int i = 0; i < victims.Length; i++) {
			victimList.Add (victims[i]);
		}
	}

	void OnDestroy()
	{
		MessageDispatcher.RemoveListener ("SEND_COPS", sendCops);
	}
	
	// Update is called once per frame
	void Update () {
		if (timerRunning) {
			updateTime ();
		}
	}

	public void sendCops(IMessage rMessage)
	{
		if (!timerRunning) {
			timerContainer.SetActive (true);
			timerRunning = true;
		}
	}

	public void updateTime()
	{
		time -= Time.deltaTime;

		//float minutes = time / 60;
		float seconds = time % 60;
		float fraction = (time * 100) % 100;

		timerLabel.text = string.Format ("{0:00}:{1:00}", seconds, fraction);
		if (time <= 0) {
			timerRunning = false;
			timerLabel.text = "00:00";
			//TODO: move lost game to when cop kills you instead of timer
			//lostGame ();
			cop.SetActive(true);
		}
	}

	public void victimKilled(GameObject victim)
	{
		victimList.Remove (victim);
		if (victimList.Count == 0) {
			showExit ();
		}
	}

	public void showExit()
	{
		exit.SetActive (true);
	}

	public void winGame()
	{
		if (cop.activeSelf) {
			CopController copScript = cop.GetComponent <CopController> ();
			copScript.giveUp ();
		}
		SceneManager.LoadScene (winScene, LoadSceneMode.Additive);
		SceneManager.MoveGameObjectToScene (sceneData, SceneManager.GetSceneByName (winScene));
	}

	public void lostGame()
	{
		//TODO make sure controls are disabled before loading scene
		SceneManager.LoadScene (loseScene, LoadSceneMode.Additive);
		SceneManager.MoveGameObjectToScene (sceneData, SceneManager.GetSceneByName (loseScene));
	}
}
