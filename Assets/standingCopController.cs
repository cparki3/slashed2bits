using UnityEngine;
using System.Collections;
using Prime31;
using com.ootii.Messages;
using UnityEngine.UI;
using com.ootii.Utilities.Debug;
using System;
using Rewired;

public class standingCopController : MonoBehaviour {

	// movement config
	[Header("Physics variables")]
	public float gravity = -25f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float walkSpeed = 1f;
	public float runSpeed  = 6f;

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

	private SpriteRenderer vicSprite;

	public GameObject victimImage;
	//public Animator victimAnimator;
	private BoxCollider2D victimCollider;
	//private GameObject playerKiller;
	private levelManagerScript levelScript;
	public GameObject soulLauncher;
	public GameObject partLauncher;
	public Rigidbody2D bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 100f;
	public float reloadTime = 2f;

	void Awake()
	{
		/*if (GameObject.Find ("PLAYER_KILLER")) {

			playerKiller = GameObject.Find ("PLAYER_KILLER");
		}*/
		levelScript = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ();
		vicSprite = victimImage.GetComponent <SpriteRenderer> ();
		victimCollider = this.GetComponent <BoxCollider2D> ();
		//_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D> ();
		sprite = this.GetComponent<SpriteRenderer> ();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		normalizedHorizontalSpeed = 0;

		//MessageDispatcher.AddListener ("SEND_COPS", setupAlert);
		//victimAnimator.SetFloat ("victimSpeed", walkSpeed);
	}

	void Start()
	{
		shoot ();
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

	public void shoot()
	{
		Rigidbody2D newBullet = Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.localRotation);
		if (victimImage.transform.localScale.x < 0) {
			Debug.Log ("shoot left");
			newBullet.transform.localScale = new Vector2 (-3, 3);
			newBullet.AddForce ((newBullet.transform.right * bulletSpeed) * -1);
		}
		else
		{
			newBullet.AddForce (newBullet.transform.right * bulletSpeed);
		}
		Invoke ("shoot", reloadTime);
	}

	public void die()
	{
		if (!isDead) {
			victimCollider.enabled = false;
			//sightContainer.SetActive (false);
			isDead = true;
			vicSprite.color = Color.white;
			//MessageDispatcher.RemoveListener ("SEND_COPS", setupAlert);
			if (!isAlert) {
				levelScript.stealthKill ();
			}
			vicSprite.enabled = false;
			Instantiate (soulLauncher, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), soulLauncher.transform.rotation);
			Instantiate (partLauncher, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), Quaternion.identity);
			Instantiate(blood, new Vector2 (this.transform.position.x, this.transform.position.y + .4f), blood.transform.rotation);
			VictimController vicController = this.GetComponent<VictimController> ();
			walkSpeed = 0;
			if (GameObject.Find ("LEVEL_MANAGER")) {
				GameObject levelManager = GameObject.Find ("LEVEL_MANAGER");
				levelManager.SendMessage ("victimKilled", this.gameObject);
			}
			Destroy(this.gameObject);
		}
	}

	private void destroyVictim()
	{
		isDead = true;
		canKill = false;
		vicSprite.color = Color.white;
		Destroy (this.gameObject);
	}

	#endregion
		
}
