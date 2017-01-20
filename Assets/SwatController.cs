using UnityEngine;
using System.Collections;
using Prime31;
using com.ootii.Messages;
using UnityEngine.UI;
using com.ootii.Utilities.Debug;
using System;
using Rewired;

public class SwatController : MonoBehaviour {

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
	public Transform startSight;
	public Transform endSight;
	private RaycastHit2D sightHit;
	public Transform startAlert;
	public Transform endAlert;
	private RaycastHit2D alertHit;

	public bool isAlert = false;
	public GameObject blood;

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
	private BoxCollider2D victimCollider;
	public bool lookForTarget = false;
	//private GameObject playerKiller;
	private levelManagerScript levelScript;
	public GameObject soulLauncher;
	public bool canShoot = true;
	public float reloadTime = 0.5f;
	public Transform bulletSpawn;
	public Rigidbody2D bulletPrefab;
	public float bulletSpeed = 600;
	public GameObject bodyPartLauncher;

	void Awake()
	{
		/*if (GameObject.Find ("PLAYER_KILLER")) {

			playerKiller = GameObject.Find ("PLAYER_KILLER");
		}*/
		slasher = GameObject.Find ("PLAYER");
		levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ();
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

		//MessageDispatcher.AddListener ("SEND_COPS", setupAlert);
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
		

	void checkForSlasher()
	{
		alertHit = Physics2D.Linecast (startAlert.position, endAlert.position, canSee.value);
		if (alertHit.collider != null) {
			//Debug.Log (sightHit.collider.tag);
			if (alertHit.collider.tag == "Player") {
				if (canShoot) {
					canShoot = false;
					shoot ();
				} else {
					//Debug.Log ("CHANGE DIRECTION!!");
					//changeDirection ();
				}
			}
		}
	}

	public void shoot()
	{
		Debug.Log ("BANG!");
		Rigidbody2D newBullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.localRotation);
		if (victimImage.transform.localScale.x < 0) {
			Debug.Log ("shoot left");
			newBullet.transform.localScale = new Vector2 (-1, 1);
			newBullet.AddForce ((newBullet.transform.right * bulletSpeed) * -1);
		}
		else
		{
			newBullet.AddForce (newBullet.transform.right * bulletSpeed);
		}
		Invoke ("resetShoot", reloadTime);
	}

	public void resetShoot()
	{
		canShoot = true;
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

	public void boostSpeed(float boostAmount)
	{
		walkSpeed += boostAmount;
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

		if (tagName == "Player") {
			//ssher.SendMessage ("die");
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
		

	public void hideHighlight()
	{
		//vicSprite.color = Color.white;
	}

	public void resetSpeed(float newSpeed)
	{
		this.walkSpeed = newSpeed;
	}

	public void die()
	{
		if (!isDead && canKill) {
			victimCollider.enabled = false;
			//sightContainer.SetActive (false);
			isDead = true;
			canKill = false;
			//vicSprite.enabled = false;
			canMove = false;
			//victimText.text = "";
			//MessageDispatcher.RemoveListener ("SEND_COPS", setupAlert);
			//Instantiate (soulLauncher, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), soulLauncher.transform.rotation);
			Instantiate(blood, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), blood.transform.rotation);
			Instantiate(bodyPartLauncher, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), Quaternion.identity);
			//victimAnimator.SetTrigger ("die");
			VictimController vicController = this.GetComponent<VictimController> ();
			walkSpeed = 0;
			if (GameObject.Find ("LEVEL_MANAGER")) {
				GameObject levelManager = GameObject.Find ("LEVEL_MANAGER");
				//levelManager.SendMessage ("victimKilled", this.gameObject);

			}
			Destroy(this.gameObject);
		}
	}

	private void destroyVictim()
	{
		isDead = true;
		canKill = false;
		vicSprite.color = Color.white;
		canMove = false;
		Destroy (this.gameObject);
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
		/*		if (playerKiller != null) {
			if (playerKiller.transform.position.y > this.transform.position.y - 10) {
				destroyVictim ();
			}
		}
		*/
		if (slasher.transform.position.y > (this.transform.position.y + 12)) {
			this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y + 2);
		}
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
		if (!isDead) {
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
				} 
			}
	}

	public void activateTarget()
	{
		if (canInterract) {
			lookForTarget = false;
			touchingObject.SendMessage ("activate");
			canMove = true;
			//victimAnimator.SetFloat ("victimSpeed", walkSpeed);
			updateUI ("");
		}
	}

	public void climbComplete()
	{
		canKill = true;
		_velocity.x = 0;
		canClimb = false;
		canMove = true;
	}	
}
