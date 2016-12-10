using UnityEngine;
using System.Collections;
/*******************************************************
 * Class:           ConeDetectionExample
 * Description:     Description begin here
 * 
 * Studio Leaves (c) 
 *******************************************************/
public class ConeDetectionExample : MonoBehaviour {

    public GameObject m_goStaticPatrolling;
    public GameObject m_goDynamicPatrolling;

    private Rect m_guiBox = new Rect( 0.0f, 0.0f, Screen.width * 0.3f, Screen.height * 0.3f );
    private Rect m_guiLabel = new Rect( ( Screen.width * 0.5f ) - 100.0f, Screen.height - 30.0f, 200.0f, 30.0f );

	void Start () {
	
	}
	void Update () {
	
	}

    void OnGUI() {

        string StaticPatrollingState = m_goStaticPatrolling.GetComponent<AIStaticPatrolling>().PatrollingState.ToString();
        string DynamicPatrollingState = m_goDynamicPatrolling.GetComponent<AIDynamicPatrolling>().DynamicPatrollingState.ToString();
        string StaticWhatIntoCone = "";
        string DynamicWhatIntoCone = "";

        foreach ( GameObject go in m_goStaticPatrolling.GetComponent<AIConeDetection>().GameObjectIntoCone ) {
            StaticWhatIntoCone += go.name + ",";
        }

        foreach ( GameObject go in m_goDynamicPatrolling.GetComponent<AIConeDetection>().GameObjectIntoCone ) {
            DynamicWhatIntoCone += go.name + ",";
        }

        GUI.Box( m_guiBox, "" );
        GUI.Label( m_guiBox, " Red Robot \n State: " + StaticPatrollingState + "\n What into Cone: " + StaticWhatIntoCone 
                            + "\n\n Blue Robot \n State: " + DynamicPatrollingState + "\n What into Cone: " + DynamicWhatIntoCone  );

        GUI.Label( m_guiLabel, "www.studioleaves.com" );
    }
}
