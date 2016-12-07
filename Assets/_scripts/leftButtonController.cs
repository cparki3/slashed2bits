using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class leftButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private slasher_touch_controls touchControls;
	// Use this for initialization
	void Awake () {
		touchControls = this.GetComponentInParent <slasher_touch_controls> ();
	}

	public void OnPointerDown (PointerEventData eventData) 
	{
		touchControls.leftPressed ();
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		touchControls.leftUp ();
	}
}
	