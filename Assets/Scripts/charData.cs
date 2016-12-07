using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class charData : MonoBehaviour {

	public GameObject charDataPrefab;
	public GameObject charAnimation;
	public Image smallCharacter;
	public GameObject checkMark;
	public bool isLocked = true;
	public Sprite unlockedSprite;
	// Use this for initialization
	void Start () {
	
	}

	public void hideCheck()
	{
		checkMark.SetActive (false);
	}

	public void showCheck()
	{
		checkMark.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
