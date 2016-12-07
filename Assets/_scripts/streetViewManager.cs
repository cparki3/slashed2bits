using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Net.Mime;

public class streetViewManager : MonoBehaviour {

	public RectTransform streetItemsContainer;
	public List<RectTransform> rectList;
	private float offLeft;
	private float offRight;
	public ScrollRect scrollArea;
	public RectTransform scrollTransform;
	public bool levelSelected = false;
	public string selectedLevel;
	public bool characterSelected = false;
	public GameObject charSelector;
	private GameManager gameScript;
	public GameObject[] buttonList;
	public GameObject[] houseButtons;
	public GameObject playButton;
	public ScrollRectEnsureVisible scrollScript;

	// Use this for initialization
	void Start () {
		gameScript = GameObject.Find ("GAME_MANAGER").GetComponent <GameManager> ();
		buttonList = GameObject.FindGameObjectsWithTag ("level_button");
		houseButtons = GameObject.FindGameObjectsWithTag ("house_buttons");
		alignChildren ();
		hideHouses ();
	}

	public void hideHouses()
	{
		for (int i = 0; i < houseButtons.Length; i++) {
			houseButtons [i].SetActive (false);
		}
	}

	public void clearChecks()
	{
		levelButton buttonScript;
		for (int i = 0; i < buttonList.Length; i++) {
			buttonScript = buttonList [i].GetComponent <levelButton> ();
			buttonScript.hideCheck ();
		}
	}

	public void hideAll()
	{
		clearChecks ();
		hideHouses ();
		hideCharSelect ();
		if (playButton != null) {
			hidePlay ();
		}

	}

	public void selectLevel(string levelName)
	{
		selectedLevel = levelName;
		Debug.Log ("LEVEL HAS BEEN SELECTED " + levelName);
		showCharSelect ();
		showPlay ();
	}
		
		
	public void showPlay()
	{
		playButton.SetActive (true);
	}

	public void hidePlay()
	{
		playButton.SetActive (false);
	}

	public void showCharSelect()
	{
		charSelector.SetActive (true);
	}

	public void hideCharSelect()
	{
		charSelector.SetActive (false);
	}

	public void loadGame()
	{
		SceneManager.LoadScene (selectedLevel);
	}

	private void alignChildren()
	{
		float imageSpacing = streetItemsContainer.GetComponent<RectTransform> ().rect.width;
		streetItemsContainer.sizeDelta = new Vector2 (imageSpacing * (rectList.Count - 1), streetItemsContainer.sizeDelta.y);
		//streetItemsContainer.GetComponent <RectTransform>().offsetMin = new Vector2(0,0);
		float startingX = 0 - (streetItemsContainer.sizeDelta.x / 2);
		offLeft = imageSpacing * -1;
		offRight = imageSpacing;
		for (int i = 0; i < rectList.Count; i++) {
			RectTransform childRect = rectList [i];
			childRect.anchoredPosition = new Vector2(startingX, 0);
			childRect.sizeDelta = new Vector2 (imageSpacing, childRect.sizeDelta.y);
			//childRect.anchorMin = new Vector2(0f, 0f);
			//childRect.anchorMax = new Vector2(.5f, .5f);
			//childRect.pivot = new Vector2(0.5f, 0.5f);


			startingX += imageSpacing;
			//imageRects.Add(childRect);
		}
		RectTransform houseRect = rectList [1];
		Debug.Log (houseRect.anchoredPosition.normalized);
		//scrollArea.normalizedPosition = new Vector2 (0, 0);
		float houseNum = 2f;
		float rectCount = rectList.Count;
		float housePos = 1 / (rectCount / houseNum);
		Debug.Log (housePos);
		scrollArea.horizontalNormalizedPosition = 0f;
		//updateColors ();
	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
