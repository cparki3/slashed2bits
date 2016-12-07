using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sceneData : MonoBehaviour {

	public int sceneIndex;

	// Use this for initialization
	void Awake () {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
