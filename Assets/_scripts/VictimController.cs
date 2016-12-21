using UnityEngine;
using System.Collections;
using Prime31;
using com.ootii.Messages;
using UnityEngine.UI;
using com.ootii.Utilities.Debug;
using System;
using Rewired;

public class VictimController : MonoBehaviour {

	// movement config
	[Header("Physics variables")]
	public float gravity = -25f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float walkSpeed = 1f;
	public float runSpeed  = 6f;

	//victim sight definitions
	[Header("Victim Sight")]
	//public GameObject sightContainer;
	public MeshRenderer visionMesh;
	public float alertVisionDistance = 1.0f;
	public Transform startSight;
	public Transform endSight;
	private RaycastHit2D sightHit;
	public Transform startAlert;
	public Transform endAlert;
	private RaycastHit2D alertHit;
	public GameObject victimCone;

	public bool isAlert = false;
	public GameObject blood;
	public Color highlightColor;
	
	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private SpriteRenderer sprite;
	
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private bool canInterract = false;
	private bool isDead = false;
	private bool canKill = true;


	
	private bool canMove = true;
	private bool canClimb = false;
	private ladder ladderScript;
	private Vector3 targetPos;
	private GameObject touchingObject = null;
	private bool hitWall = false;
	private Vector3 visionRight = new Vector3 (0, 0, -90);
	private Vector3 visionLeft = new Vector3(0,0,90);
	public Text victimText;
	private SpriteRenderer vicSprite;
	private GameObject slasher;
	private int slasherPos = 0;

	private GameObject fuseBox;
	public Transform target;
	public LayerMask canSee;
	public GameObject victimImage;
	public Animator victimAnimator;
	public GameObject victimEyelids;
	private BoxCollider2D victimCollider;
	public bool lookForTarget = false;
	
	void Awake()
	{
		lookForTarget = false;
		vicSprite = victimImage.GetComponent <SpriteRenderer> ();
		victimCollider = this.GetComponent <BoxCollider2D> ();
		//_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D> ();
		sprite = this.GetComponent<SpriteRenderer> ();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerStayEvent += onTriggerStayEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		normalizedHorizontalSpeed = 1;
		MessageDispatcher.AddListener ("SEND_COPS", setupAlert);
		victimAnimator.SetFloat ("victimSpeed", walkSpeed);
	}
	
	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}

	public void setupAlert(IMessage rMessage)
	{
		isAlert = true;
		slasher = GameObject.Find ("PLAYER");
		walkSpeed = runSpeed;
		victimText.text = "!";
		victimAnimator.SetFloat ("victimSpeed", walkSpeed);
		MessageDispatcher.RemoveListener ("SEND_COPS", setupAlert);
	}
		
	void checkForSlasher()
	{
		alertHit = Physics2D.Linecast (startAlert.position, endAlert.position, canSee.value);
		if (alertHit.collider != null) {
			//Debug.Log (sightHit.collider.tag);
			if (alertHit.collider.tag == "Player") {
				if (!isAlert) {
					//Debug.Log ("OH GOD ITS A MURDERER SAVE ME!");
					MessageDispatcher.SendMessage ("SEND_COPS");
					//changeDirection ();
				} else {
					//Debug.Log ("CHANGE DIRECTION!!");
					changeDirection ();
				}
			}
		}
	}

	public void changeDirection()
	{
		if (normalizedHorizontalSpeed > 0) {
			victimImage.transform.localScale = new Vector2 (-1, 1);
			normalizedHorizontalSpeed = walkSpeed * -1;
			//sightContainer.transform.eulerAngles = visionLeft;
		} else {
			victimImage.transform.localScale = new Vector2 (1, 1);
			normalizedHorizontalSpeed = walkSpeed;
			//sightContainer.transform.eulerAngles = visionRight;
		}
		hitWall = false;
	}
		
	
	void onTriggerStayEvent( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		if(tagName == "climbable")
		{
			ladderScript = col.gameObject.GetComponent<ladder>();
			canClimb = true;
			touchingObject = col.gameObject;
		}
		canInterract = true;

		if (tagName == "darkness" && lookForTarget == false) {
			touchingObject = col.gameObject;
			fuseBox = touchingObject.GetComponentInParent <lightScript> ().fuseBox;
			target = fuseBox.transform;
			updateUI ("?");
		//	Debug.Log ("DARK!!!!!");
			lookForTarget = true;
		}
		//Debug.Log (tagName);
		if (tagName != "darkness") {
			
			touchingObject = col.gameObject;
		} else {
		//	Debug.Log ("TOUCHING DARKNESS!!");
		}
	}

	public void updateUI(string text)
	{
		victimText.text = text;
	}
	
	public void showHighlight()
	{
		if (canKill) {
			vicSprite.color = highlightColor;
		}
	}
	
	public void hideHighlight()
	{
		vicSprite.color = Color.white;
	}
	
	public void die()
	{
		if (!isDead && canKill) {
			victimCollider.enabled = false;
			//sightContainer.SetActive (false);
			isDead = true;
			canKill = false;
			victimCone.SendMessage ("destroyCone", null, SendMessageOptions.DontRequireReceiver);
			vicSprite.color = Color.white;
			canMove = false;
			victimText.text = "";
			MessageDispatcher.RemoveListener ("SEND_COPS", setupAlert);
			Instantiate(blood, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), blood.transform.rotation);
			victimEyelids.SetActive (false);
			victimAnimator.SetTrigger ("die");
			VictimController vicController = this.GetComponent<VictimController> ();
			walkSpeed = 0;
			if (GameObject.Find ("LEVEL_MANAGER")) {
				GameObject levelManager = GameObject.Find ("LEVEL_MANAGER");
				levelManager.SendMessage ("victimKilled", this.gameObject);
			}
			//Destroy(this.gameObject);
		}
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		if(tagName == "climbable")
		{
			canClimb = false;
		}
		canInterract = false;
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

	}
	
	#endregion
	
	
	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void FixedUpdate()
	{
		if(canMove && !isDead)
		{
			checkForSlasher ();
			checkMovement();
		}
		//Debug.DrawLine (startSight.position, endSight.position, Color.green);
		sightHit = Physics2D.Linecast (startSight.position, endSight.position, canSee.value);
		if (sightHit.collider != null) {
			//Debug.Log (sightHit.collider.tag);
			if (sightHit.collider.tag == "wall" && !hitWall) {
				hitWall = true;
				changeDirection ();
			}
		}
	}
	
	void checkMovement()
	{

		if( _controller.isGrounded )
			_velocity.y = 0;
			
		if(canMove)
		{
			checkInterraction ();
		}
		else
		{
			
			normalizedHorizontalSpeed = 0;
			//_animator.SetBool("isWalking", false);
			
		}
		
		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * walkSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		
		
		_controller.move( _velocity * Time.deltaTime );
		
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}
	
	public void climb()
	{
		canKill = false;
		Vector3 startPos;
		
		Vector3 bottom = ladderScript.bottomTarget.transform.position;
		Vector3 top = ladderScript.topTarget.transform.position;
		float distTop = Vector3.Distance(transform.position, ladderScript.topTarget.transform.position);
		float distBottom = Vector3.Distance(transform.position, ladderScript.bottomTarget.transform.position);
		
		if(distBottom < distTop)
		{
			//Debug.Log ("climb up");
			startPos = bottom;
			targetPos = top;
		}
		else{
			//Debug.Log ("climb down");
			startPos = top;
			targetPos = bottom;
		}
		
		LeanTween.moveX (this.gameObject, startPos.x, .1f).setOnComplete(climbY);
		
	}
	
	public void climbY(){
		LeanTween.moveY (this.gameObject, targetPos.y, .3f).setOnComplete(climbComplete);
	}

	void checkInterraction()
	{
		if (isAlert) {
			if (slasher.transform.position.y > (this.transform.position.y + 1)) {
				slasherPos = 1;
			}
			if (slasher.transform.position.y < (this.transform.position.y - 1)) {
				slasherPos = -1;
			}
			if (Math.Abs (slasher.transform.position.y - this.transform.position.y) < 1) {
				slasherPos = 0;
			}
			if (slasherPos > 0) {
				if (canClimb) {
					if (ladderScript.bottomTarget.position.y < (this.transform.position.y - 1)) {
						canClimb = false;
						canMove = false;
						climb ();
					}
				}
			} else if (slasherPos < 0) {
				if (canClimb) {
					if (ladderScript.topTarget.position.y > (this.transform.position.y + 1)) {
						canClimb = false;
						canMove = false;
						climb ();
					}
				}
			} else {
				if (canClimb) {
					canClimb = false;
					canMove = false;
					climb ();
				}
			}
		} else {
			if (lookForTarget) {
				findTarget ();
			}
		}

	}

	private void findTarget()
	{
		int targetPos = 0;
		if (target.position.y > (this.transform.position.y + 1)) {
			targetPos = 1;
		}
		if (target.position.y < (this.transform.position.y - 1)) {
			targetPos = -1;
		}
		if (Math.Abs (target.position.y - this.transform.position.y) < 1) {
			targetPos = 0;
		}
		if (targetPos < 0) {
			if (canClimb) {
				if (ladderScript.bottomTarget.position.y < (this.transform.position.y - 1)) {
					canClimb = false;
					canMove = false;
					climb ();
				}
			}
		} else if (targetPos > 0) {
			if (canClimb) {
				if (ladderScript.topTarget.position.y > (this.transform.position.y + 1)) {
					canClimb = false;
					canMove = false;
					climb ();
				}
			}
		} else {
			if (Math.Abs (target.position.x - this.transform.position.x) < .01) {
				victimAnimator.SetFloat ("victimSpeed", 0f);
				canMove = false;
				Invoke ("activateTarget", 1f);
				//Debug.Log ("found target");
			}
		}
	}

	public void activateTarget()
	{
		if (canInterract) {
			lookForTarget = false;
			touchingObject.SendMessage ("activate");
			canMove = true;
			victimAnimator.SetFloat ("victimSpeed", walkSpeed);
			updateUI ("");
		}
	}
	
	public void climbComplete()
	{
		canKill = true;
		_velocity.x = 0;
		canClimb = true;
		canMove = true;
	}	
}
