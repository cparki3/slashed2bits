using UnityEngine;
using System.Collections;

public class EnemyAIStaticPatrolling : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ( this.GetComponent<AIStaticPatrolling>().PatrollingState == AIStaticPatrolling.STATICPATROLLINGSTATE.PS_WAITING ) {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
	}
}
