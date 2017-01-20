using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyPartLauncher : MonoBehaviour {


	public GameObject[] launchItems;
	public int itemsToLauch = 5;
	public float launchDelay = .3f;
	public bool launchAtStart = false;
	public float launchX = 300;
	public float launchY = 1000;
	public bool launchAll = false;
	// Use this for initialization
	void Awake () {
		//if (GameObject.Find ("LEVEL_MANAGER")) {
			//itemsToLauch = GameObject.Find ("LEVEL_MANAGER").GetComponent <levelManagerScript> ().stealthMultiplier;
		//}
		if(launchAtStart)
		{
			if(launchAll)
			{
				launchAllItems();
			}
			else
			{
				activate();
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void activate()
	{
		//launchItem();
	}

	public void launchAllItems()
	{
		for(int i = 0; i < launchItems.Length; i++)
		{
			float randomLaunch  = launchY - (Random.value * 400);
			Vector3 randomPosition = new Vector3(Random.Range(this.transform.position.x - 1,this.transform.position.x + 1),transform.position.y);

			float randomX = Random.Range(launchX * -1, launchX);
			GameObject item = Instantiate(launchItems[i], transform.position, transform.rotation) as GameObject;
			//Debug.Log(transform.rotation.eulerAngles);
			int currentRotation = Mathf.FloorToInt(transform.rotation.eulerAngles.y);
			//Debug.Log("MOD = " + (currentRotation % 90));
			if(currentRotation % 90 == 0 && currentRotation % 180 != 0)
			{
				item.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, randomLaunch, randomX));
			}
			else
			{
				item.GetComponent<Rigidbody2D>().AddForce(new Vector3(randomX, randomLaunch, 0));
			}


		}
		//Destroy(gameObject,1f);
	}

	private void launchItem()
	{
		if(itemsToLauch > 0)
		{
			float randomX = Random.Range(launchX * -1, launchX);
			int itemInt = Random.Range(0,(launchItems.Length-1));
			GameObject item = Instantiate(launchItems[itemInt], transform.position, transform.rotation) as GameObject;
			item.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX, launchY));
			itemsToLauch --;
			//Debug.Log("should be launching a money item!");
			Invoke("launchItem", launchDelay);
		}
	}
}
