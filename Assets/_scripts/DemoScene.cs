using UnityEngine;
using System.Collections;
using Prime31;
using Rewired;
using Rewired.Platforms;
using System.Security.Cryptography.X509Certificates;
using System;
using Com.LuisPedroFonseca.ProCamera2D;


public class DemoScene : MonoBehaviour
{
	// movement config
	public int playerId = 0;
	private Player player;
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public bool moveRight = false;
	public bool moveLeft = false;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private SpriteRenderer sprite;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private Rigidbody2D rb2d;
	private bool canInterract = false;
	
	public bool canMove = false;
	private bool canClimb = false;
	private ladder ladderScript;
	private Vector3 targetPos;
	private GameObject touchingObject = null;
	public GameObject defaultSlasher;
	public GameObject portal;
	private Color startColor;
	private GameObject newPortal;
	private GameObject slasher;
	private GameManager GM;
	private bool canKill = false;
	private GameObject victim;
	private levelManagerScript levelScript;
	private BoxCollider2D playerCol;
	private ProCamera2D proCam;
	private ProCamera2DShake proShake;


	void Awake()
	{
		startColor = this.GetComponent <SpriteRenderer> ().color;
		this.GetComponent <SpriteRenderer>().enabled = false;
		player = ReInput.players.GetPlayer (playerId);

		_controller = GetComponent<CharacterController2D>();
		playerCol = this.GetComponent<BoxCollider2D> ();
		rb2d = this.GetComponent <Rigidbody2D> ();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerStayEvent += onTriggerStayEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		proCam = Camera.main.GetComponent <ProCamera2D> ();
		proShake = Camera.main.GetComponent <ProCamera2DShake> ();
		levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ();
		//newPortal = Instantiate (portal, this.transform.position, Quaternion.identity) as GameObject;
		Invoke ("addPlayer", 1f);

	}

	public void addPlayer()
	{
		if (GameObject.Find ("GAME_MANAGER")) {
			GM = GameObject.Find ("GAME_MANAGER").GetComponent <GameManager> ();
			if (GM.player1 != null) {
				slasher = Instantiate (GM.player1, this.transform.position, Quaternion.identity) as GameObject;
			} else {
				slasher = Instantiate (defaultSlasher, this.transform.position, Quaternion.identity) as GameObject;
			}
		} else {
			slasher = Instantiate (defaultSlasher, this.transform.position, Quaternion.identity) as GameObject;
		}
		sprite = slasher.GetComponent<SpriteRenderer>();
		sprite.color = startColor;
		slasher.transform.parent = this.transform;
		slasher.transform.position = new Vector2 (slasher.transform.position.x, slasher.transform.position.y + .5f);
		LeanTween.moveLocalY (slasher, 0f, .5f);
		LeanTween.color (slasher, Color.white, .5f);
		LeanTween.scaleX (slasher, 1f, .5f).setOnComplete (movePlayer);
		LeanTween.scaleY (slasher, 1f, .5f);
		_animator = slasher.GetComponent<Animator>();

	}

	public void movePlayer()
	{
		Invoke("removePortal", .5f);
	}

	public void removePortal()
	{
		Animator portalAnimator = portal.GetComponentInChildren <Animator> ();
		portalAnimator.SetTrigger ("leave");
		Invoke ("destroyPortal", .5f);
	}

	public void destroyPortal()
	{
		canMove = true;
		Destroy (portal, 1f);
	}

	public void stopMovement()
	{
		_animator.SetBool("isWalking", false);
		canMove = false;
	}

	public void exitPortal()
	{
		LeanTween.moveLocalY (slasher, 0.5f, .5f);
		LeanTween.color (slasher, Color.black, .5f);
		LeanTween.scaleX (slasher, 0f, .5f).setOnComplete(leftPortal);
		LeanTween.scaleY (slasher, 0f, .5f);
	}

	public void leftPortal()
	{

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


	void onTriggerStayEvent( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.tag );
		switch (tagName) {
		case "climbable":
			ladderScript = col.gameObject.GetComponent<ladder>();
			canClimb = true;
			break;
		case "victim":
			victim = col.gameObject;
			canKill = true;
			break;
		case "kill_bar":
			die ();
			break;
		}
		canInterract = true;
		touchingObject = col.gameObject;
		col.gameObject.SendMessage("showHighlight", null, SendMessageOptions.DontRequireReceiver);

	}


	void onTriggerExitEvent( Collider2D col )
	{
		string tagName = col.gameObject.tag;
		switch (tagName) {
		case "climbable":
			canClimb = false;
			break;
		case "victim":
			canKill = false;
			break;
		}

		canInterract = false;
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
		col.gameObject.SendMessage("hideHighlight", null, SendMessageOptions.DontRequireReceiver);
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		if(canMove)
		{
			checkMovement();
		}

		if (player.GetButtonDown ("Jump")) {
			checkInterraction ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			checkInterraction ();
		}
		
	}
	
	void checkMovement()
	{
		if( _controller.isGrounded )
			_velocity.y = 0;
		
		if( Input.GetKey( KeyCode.RightArrow ) || moveRight )
		{
			_animator.SetBool("isWalking", true);
			normalizedHorizontalSpeed = 1;
			if( sprite.flipX == true )
			{
				sprite.flipX = false;
				
			}
			
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) || moveLeft )
		{
			_animator.SetBool("isWalking", true);
			normalizedHorizontalSpeed = -1;
			if( sprite.flipX == false )
			{
				sprite.flipX = true;
			}
			
		}
		else if (player.GetAxis ("Move X") > 0f) {
			_animator.SetBool("isWalking", true);
			normalizedHorizontalSpeed = 1;
			if( sprite.flipX == true )
			{
				sprite.flipX = false;

			}
		}

		else if (player.GetAxis ("Move X") < 0f) {
			_animator.SetBool("isWalking", true);
			normalizedHorizontalSpeed = -1;
			if( sprite.flipX == false )
			{
				sprite.flipX = true;
			}
		}
		else
		{
			
			normalizedHorizontalSpeed = 0;
			_animator.SetBool("isWalking", false);
			
		}
	

		
		
		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		
		
		_controller.move( _velocity * Time.deltaTime );
		
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

	public void copsFoundYou()
	{
		_animator.SetBool ("isWalking", false);
	}

	public void checkInterraction()
	{
		//check if can perform action
		if(canInterract)
		{

			Debug.Log ("DO SOMETHING");
			if (canKill) {
				int randomAnim = UnityEngine.Random.Range (1, 10);
				if (randomAnim % 2 == 0) {
					_animator.SetTrigger ("attack1");
				} else {
					_animator.SetTrigger ("attack2");
				}
				proShake.Shake ();
				victim.SendMessage ("die");
				canKill = false;
				return;
			}
			if(canClimb)
			{
				canClimb = false;
				canMove = false;
				climb();
			}
			else
			{
				touchingObject.SendMessage("activate", null, SendMessageOptions.DontRequireReceiver);
			}
		}
		
	}

	public void die()
	{
		_animator.SetTrigger ("shot");
		proShake.Shake ();
		playerCol.enabled = false;
		canInterract = false;
		levelScript.playerDead ();
	}
	
	public void climb()
	{
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
	
	public void climbY(){
		LeanTween.move(this.gameObject, targetPos, .3f).setOnComplete(climbComplete);
	}

	public void hide()
	{
		_velocity.x = 0;
		sprite.enabled = false;
		playerCol.enabled = false;
		canMove = false;
	}

	public void show()
	{
		_velocity.x = 0;
		sprite.enabled = true;
		playerCol.enabled = true;
		canMove = true;

	}
	
	public void climbComplete()
	{
		_velocity.x = 0;
		canClimb = false;
		canMove = true;
	}
	
}
