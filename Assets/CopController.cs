using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prime31;
using com.ootii.Messages;
using System;

public class CopController : MonoBehaviour {

	// movement config
	[Header("Physics vars")]
	public float gravity = -25f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float walkSpeed = 1f;
	public float runSpeed  = 6f;

	[Header("Other vars")]
	public bool isAlert = true;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private SpriteRenderer sprite;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private bool canInterract = false;
	private bool isDead = false;
	private bool canKill = false;

	private bool canMove = true;
	private bool canClimb = false;
	private ladder ladderScript;
	private Vector3 targetPos;
	private GameObject touchingObject = null;
	private bool hitWall = false;
	private SpriteRenderer vicSprite;
	private GameObject slasher;
	private int slasherPos = 0;
	private DemoScene slashScript;



	public LayerMask canSee;
	public GameObject victimImage;
	public Animator victimAnimator;
	public GameObject victimEyelids;
	private BoxCollider2D victimCollider;
	public bool foundSlasher = false;

	//STANDARD SIGHT STUFF
	[Header("Sight variables")]
	public Transform startSight;
	public Transform endSight;
	private RaycastHit2D sightHit;
	private RaycastHit2D shootHit;
	public Transform startShoot;
	public Transform endShoot;



	void Awake()
	{
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
		//MessageDispatcher.AddListener ("SEND_COPS", setupAlert);
		victimAnimator.SetFloat ("walkSpeed", walkSpeed);
		slasher = GameObject.Find ("PLAYER");
		slashScript = slasher.GetComponent <DemoScene> ();
		walkSpeed = runSpeed;
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
		

	//REPLACE 
	/*void sawSomething(GameObject[] objects)
	{
		foreach(GameObject item in objects)
		{
			if (item.name == "PLAYER") {
				if (!isAlert) {
					isAlert = true;
					slasher = item;
					Debug.Log ("OH GOD ITS A MURDERER SAVE ME!");
					MessageDispatcher.SendMessage ("SEND_COPS");

				} else {
					Debug.Log ("CHANGE DIRECTION!!");
					changeDirection ();
				}
			}
		}
	}*/


	public void changeDirection()
	{
		if (normalizedHorizontalSpeed > 0) {
			victimImage.transform.localScale = new Vector2 (-1, 1);
			normalizedHorizontalSpeed = walkSpeed * -1;
		} else {
			victimImage.transform.localScale = new Vector2 (1, 1);
			normalizedHorizontalSpeed = walkSpeed;
		}
		hitWall = false;
	}


	void onTriggerStayEvent( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.tag );
		if(tagName == "climbable")
		{
			ladderScript = col.gameObject.GetComponent<ladder>();
			canClimb = true;
		}
		canInterract = true;
		touchingObject = col.gameObject;
	}

	public void giveUp()
	{
		isDead = true;
		canMove = false;
		walkSpeed = 0;
	}

	//TODO COP CANT DIE BUT MAY REVISIT THIS SECTION
	public void die()
	{
		if (!isDead && canKill) {
			victimCollider.enabled = false;
			isDead = true;
			canKill = false;
			vicSprite.color = Color.white;

			canMove = false;
			//MessageDispatcher.RemoveListener ("SEND_COPS", setupAlert);
			//Instantiate(blood, this.transform.position, Quaternion.identity);
			victimEyelids.SetActive (false);
			victimAnimator.SetTrigger ("die");
			//VictimController vicController = this.GetComponent<VictimController> ();
			walkSpeed = 0;
			//Destroy(this.gameObject);
		}
	}

	//TOGGLES OFF INTERACTABLE ITEMS 
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
			checkMovement();
		}
		//Debug.DrawLine (startSight.position, endSight.position, Color.green);
		//CHECKS WHAT COP CAN SEE
		sightHit = Physics2D.Linecast (startSight.position, endSight.position, canSee.value);
		if (sightHit.collider != null) {
			//Debug.Log (sightHit.collider.tag);
			if (sightHit.collider.tag == "wall" && !hitWall) {
				//TURNS CHARACTER WHEN WALL GETS HIT
				hitWall = true;
				changeDirection ();
			}
		}
		shootHit = Physics2D.Linecast (startShoot.position, endShoot.position, canSee.value);
		if (shootHit.collider != null && !foundSlasher) {
			if (shootHit.collider.tag == "Player") {
				foundSlasher = true;
				killSlasher ();
			}
		}
	}

	void killSlasher()
	{
		canMove = false;
		slashScript.canMove = false;
		victimAnimator.SetTrigger ("shoot");
		slashScript.copsFoundYou ();
	}

	public void fireWeapon()
	{
		Debug.Log ("SHOOT THE MONSTER!");
		slashScript.die ();
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

	//CLIMB UP LADDER 
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
			Debug.Log ("climb up");
			startPos = bottom;
			targetPos = top;
		}
		else{
			Debug.Log ("climb down");
			startPos = top;
			targetPos = bottom;
		}

		LeanTween.moveX (this.gameObject, startPos.x, .1f).setOnComplete(climbY);

	}

	//INITATE CLIMB ANIMATION 
	public void climbY(){
		LeanTween.moveY (this.gameObject, targetPos.y, .3f).setOnComplete(climbComplete);
	}

	//WHEN CLIMB ANIMATION IS COMPLETE
	public void climbComplete()
	{
		//canKill = true;
		_velocity.x = 0;
		canClimb = true;
		canMove = true;
	}	

	//CHECKS IF CAN INTERRACT WITH THE ITEM
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
			if (slasherPos < 0) {
				if (canClimb) {
					if (ladderScript.bottomTarget.position.y < (this.transform.position.y - 1)) {
						canClimb = false;
						canMove = false;
						climb ();
					}
				}
			} else if (slasherPos > 0) {
				if (canClimb) {
					if (ladderScript.topTarget.position.y > (this.transform.position.y + 1)) {
						canClimb = false;
						canMove = false;
						climb ();
					}
				}
			} else {
				// Slasher is on the same floor
				//Debug.Log("checking for slasher");
				/*if (canClimb) {
					canClimb = false;
					canMove = false;
					climb ();
				}*/
			}
		}

	}
}
