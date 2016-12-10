using UnityEngine;
using System.Collections;

public class EnemyAIDynamicPatrolling : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
        if ( this.GetComponent<AIDynamicPatrolling>().DynamicPatrollingState == AIDynamicPatrolling.DPATROLLINGSTATE.PS_WAITING ) {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            this.GetComponent<Renderer>().material.color = Color.green;
        }

        Debug.DrawLine( this.transform.position, this.GetComponent<AIDynamicPatrolling>().NextPointToReach, Color.white );
	}
}
