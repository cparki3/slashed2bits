using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class CharacterSelect : MonoBehaviour {

	private ScrollRectEnsureVisible scrollScipt;
	private ScrollRect scroll;
	public Transform imageContainer;
	private List<RectTransform> imageRects = new List<RectTransform>();
	private float showPosition = 0;
	private float offLeft;
	private float offRight;
	public int currentChild;
	public int playerNum;
	public bool next = false;
	public float transitionSpeed = .1f;
	public charData charInfo;
	public Button nextButton;
	public Button prevButton;
	public Color hiddenColor;
	public Sprite selectedChar;
	public Sprite deselectedChar;

	private CharacterPanel charPanel;
	// Use this for initialization
	void Start () {
		//scrollScipt = this.GetComponent<ScrollRectEnsureVisible> ();
		scroll = this.GetComponent<ScrollRect> ();
		alignChildren ();
		showCurrentChild ();
	}

	private void alignChildren()
	{
		float imageSpacing = imageContainer.GetComponent<RectTransform> ().rect.width;
		float startingX = 0;
		offLeft = imageSpacing * -1;
		offRight = imageSpacing;
		for (int i = 0; i < imageContainer.childCount; i++) {
			RectTransform childRect = imageContainer.GetChild(i).GetComponent<RectTransform>();

			childRect.anchoredPosition = new Vector2(startingX, 0);
			startingX += imageSpacing;
			imageRects.Add(childRect);
		}
		updateColors ();
	}

	public void prevChild()
	{
		next = false;
		MoveItems ();
	}

	public void nextChild()
	{
		next = true;
		MoveItems ();
	}

	private void disableButtons()
	{
		nextButton.interactable = false;
		prevButton.interactable = false;
	}

	private void updateColors()
	{
		for (int i = 0; i < imageRects.Count; i++) {
			Image charImage = imageRects [i].GetComponent <Image> ();
			charData currentChar = imageRects [i].GetComponent <charData> ();
			if (i == currentChild) {
				currentChar.smallCharacter.color = Color.white;
				currentChar.showCheck ();
				//charImage.sprite = selectedChar;
			} else {
				currentChar.hideCheck ();
				currentChar.smallCharacter.color = hiddenColor;
				//charImage.sprite = deselectedChar;
			}
		}
		charInfo = imageRects [currentChild].GetComponent<charData> ();
	}

	private void moveComplete()
	{
		if (currentChild == 0) {
			prevButton.interactable = false;
		} else {
			prevButton.interactable = true;
		}

		if (currentChild == imageRects.Count - 1) {
			nextButton.interactable = false;
		} else {
			nextButton.interactable = true;
		}
		updateColors ();
	}
		

	//Use this method to eventually load previously selected character into currently selected spot
	private void showCurrentChild()
	{
		// simply aligns first child right now
		currentChild = 0;
		//Debug.Log (imageContainer.childCount - 1);
		imageRects [currentChild].anchoredPosition = new Vector2 (showPosition, 0);
		charInfo = imageRects [currentChild].GetComponent <charData> ();
	}

	void MoveItems () {


		if (next) {
			if (currentChild != 0) {
				disableButtons ();
				currentChild--;
				Debug.Log ("Move all this direction");
				for (int i = 0; i < imageRects.Count; i++) {
					LeanTween.moveX (imageRects [i], (imageRects [i].anchoredPosition.x + offRight), transitionSpeed).setOnComplete (moveComplete);
				}
			} else {
				Debug.Log ("can't move anymore this way");
			}
		} else {
			if (currentChild < (imageRects.Count -1)) {
				disableButtons ();
				currentChild++;
				Debug.Log ("move all items to the left");
				for (int i = 0; i < imageRects.Count; i++) {
					LeanTween.moveX (imageRects [i], (imageRects[i].anchoredPosition.x - offRight), transitionSpeed).setOnComplete(moveComplete);
				}
			} else {
				Debug.Log ("cant move this way anymore");
			}
		}
		/*
		if (next) {
			if (currentChild != 0 && currentChild != imageContainer.childCount - 1) {
				currentChild++;
				LeanTween.moveX (imageRects [currentChild - 1], offLeft, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				if (currentChild + 1 != imageContainer.childCount) {
					//imageRects [currentChild + 1].localPosition = offRight;
				}
			} else if (currentChild == 0) {
				currentChild++;
				//imageRects [currentChild].localPosition = offRight;
				LeanTween.moveX (imageRects [0], offLeft, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				//imageRects [currentChild + 1].localPosition = offRight;
			} else {
				currentChild = 0;
			//	imageRects [currentChild].localPosition = offRight;
				LeanTween.moveX (imageRects [imageContainer.childCount - 1], offLeft, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				//imageRects [currentChild + 1].localPosition = offRight;
			}
		} else {
			if (currentChild != 0 && currentChild != imageContainer.childCount - 1) {
				currentChild--;
				LeanTween.moveX (imageRects [currentChild + 1], offRight, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				if (currentChild != 0) {
					//imageRects [currentChild - 1].localPosition = offLeft;
				}
			} else if (currentChild == 0) {
				currentChild = imageContainer.childCount -1;
				//imageRects [currentChild].localPosition = offLeft;
				LeanTween.moveX (imageRects [0], offRight, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				//imageRects [currentChild - 1].localPosition = offLeft;
			} else {
				currentChild --;
				//imageRects [currentChild].localPosition = offLeft;
				LeanTween.moveX (imageRects [imageContainer.childCount - 1], offRight, transitionSpeed);
				LeanTween.moveX (imageRects [currentChild], showPosition, transitionSpeed);
				//imageRects [currentChild - 1].localPosition = offLeft;
			}
		}
*/

	}
	
}
