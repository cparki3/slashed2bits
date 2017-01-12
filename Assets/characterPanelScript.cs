using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterPanelScript : MonoBehaviour {

	public characterData[] characters;

	public Image characterImage;
	public Image charButtonImage;
	public Text characterName;
	private GameManager GM;
	private int charIndex = 0;

	// Use this for initialization
	void Start () {
		GM = GameObject.Find ("GAME_MANAGER").GetComponent <GameManager> ();

		if (GM.selectedCharacter != null) {
			characterImage.sprite = GM.selectedCharacter.idolImage;
			characterName.text = GM.selectedCharacter.name;
		} else {
			characterImage.sprite = characters [0].idolImage;
			characterName.text = characters [0].characterName;
		}
	}

	public void showCharacter()
	{
		characterImage.sprite = characters [charIndex].idolImage;
		characterName.text = characters [charIndex].characterName;
	}

	public void chooseNext()
	{
		if (charIndex == characters.Length - 1) {
			charIndex = 0;
		} else {
			charIndex ++;
		}
		showCharacter ();
	}

	public void choosePrevious()
	{
		if (charIndex != 0) {
			charIndex --;
		} else {
			charIndex = characters.Length -1;
		}
		showCharacter ();
	}

	public void selectCharacter()
	{
		GM.selectedCharacter = characters [charIndex];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
