using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelButton : MonoBehaviour {

	public bool isLocked = false;
	private string levelScene;
	public int levelNum;
	public Text levelText;
	private Image btnImage;
	public Sprite lockedImg;
	public streetViewManager streetScript;
	public GameObject checkMark;

	// Use this for initialization
	void Awake () {
		levelScene = "level_" + levelNum;
		levelText.text = levelNum.ToString ();
		btnImage = this.GetComponent <Image> ();
		if (isLocked) {
			btnImage.sprite = lockedImg;
		}
	 }
	
	// Update is called once per frame
	void Update () {

	}

	public void hideCheck()
	{
		checkMark.SetActive (false);
	}

	public void selectLevel()
	{
		streetScript.clearChecks ();
		checkMark.SetActive (true);
		//streetScript.selectedLevel = levelScene;
		streetScript.selectLevel (levelScene);
	}

	public void loadScene()
	{
		if (!isLocked) {
			SceneManager.LoadScene (levelScene);
		}
	}
}
