using UnityEngine;
using System.Collections;

public class lose_animation_controller : MonoBehaviour {

	public GameObject losePanel;
	public Animator startAnimator;
	public Animator bloodAnimator;
	public Animator pipePanelAnim;

	// Use this for initialization
	void Awake () {
		losePanel.SetActive (true);
		startAnimator = losePanel.GetComponent <Animator> ();
		startAnimator.SetTrigger ("showIntro");
	}

	public void introComplete()
	{
		bloodAnimator.SetTrigger ("bleed");
	}

	public void bloodComplete()
	{
		pipePanelAnim.SetTrigger ("showMenu");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
