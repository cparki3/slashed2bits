using UnityEngine;
using System.Collections;

/*******************************************************
 * Class:           AIDynamicPatrolling
 * Description:     Description begin here
 * 
 * Studio Leaves (c) 
 *******************************************************/
public class AIDynamicPatrolling : MonoBehaviour {

    public enum DPATROLLINGSTATE {
        PS_WALKING
      , PS_WAITING
    }

    public  GameObject[] 	 m_PatrolPath;
    public  float[]      	 m_PatrolPathWaitTiming;
    public  bool 			 m_Loop					 = true;
    public 	float 			 m_fSpeed 				 = 2.0f;

    private int 		 	 m_PatrolPathLenght 	 = 0;
    private int 		 	 m_PatrolCurrentStep 	 = 0;
    private float 		 	 m_DistanceLimit 		 = 1.2f;
    private float 			 m_vOriginalYPosition;
    private float 		 	 m_fTimeToWait;
    private GameObject 	 	 m_PatrolCheckPoint 	 = null;
    private TimeCountManager m_tcmTimeManager;
    private Vector3      	 m_vCurrentPosition;
    private Vector3          m_vDirectionToLook      = new Vector3( 0.0f, 0.0f, 0.0f );
    private bool 		 	 m_Backward 			 = false;
    private bool 		 	 m_bAlreadySetted 		 = false;
    private bool			 m_bWarningEnded 		 = false;
    private bool             m_bStartPatrolling      = true;
    private DPATROLLINGSTATE m_enumPatrollingState   = DPATROLLINGSTATE.PS_WAITING;

    public Vector3 DirectionToLook {
        get { return m_vDirectionToLook; }
    }
    public Vector3 NextPointToReach {
        get { return m_PatrolPath[ m_PatrolCurrentStep ].transform.position; }
    }

    public bool ActivePatrolling {
        set { m_bStartPatrolling = value; }
        get { return m_bStartPatrolling;  }
    }

    public DPATROLLINGSTATE DynamicPatrollingState {
        get { return m_enumPatrollingState; }
    }

	void Start () {
        InitAIDynamicPatrolling();
	}
	
	void Update () {
        if ( m_bStartPatrolling ) {
            if ( m_PatrolPath.Length > 0 ) {
                PerformWalking();
            }
        }
	}

    private void InitAIDynamicPatrolling() {
        if ( m_PatrolPath.Length != m_PatrolPathWaitTiming.Length ) {
            Debug.LogError( "PatrolPath lenght and PatrolPathWaitTiming lenght must be the same!" );
            Debug.Break();
        }
        m_vCurrentPosition      = this.transform.position;
        m_vOriginalYPosition    = this.transform.position.y;
        m_PatrolPathLenght      = m_PatrolPath.Length;

        if ( m_PatrolPath.Length > 0 ) {
            m_PatrolCheckPoint  = m_PatrolPath[ m_PatrolCurrentStep ];
            m_vDirectionToLook  = m_PatrolCheckPoint.transform.position - this.transform.position;
            //m_fTimeToWait       = m_PatrolPathWaitTiming[ m_PatrolCurrentStep ];
        }

        m_tcmTimeManager = new TimeCountManager();
    }

    public void PerformWalking() {
		
		if ( m_tcmTimeManager.TimePassed > m_fTimeToWait || m_bWarningEnded ) {
			if ( m_bAlreadySetted )
				m_enumPatrollingState = DPATROLLINGSTATE.PS_WALKING;

			m_bAlreadySetted 		 	= false;
			m_vCurrentPosition 	    	+= m_vDirectionToLook * m_fSpeed * Time.deltaTime;
			
			m_vCurrentPosition.y     	= m_vOriginalYPosition;
			this.transform.position  	= m_vCurrentPosition;
			Vector3 vForward 			= this.transform.forward;
			vForward.x                  = Mathf.Lerp( vForward.x, m_vDirectionToLook.x, Time.deltaTime * 30.2f );
			vForward.z                  = Mathf.Lerp( vForward.z, m_vDirectionToLook.z, Time.deltaTime * 30.2f );
			this.transform.forward      = vForward; 
		}
		
		if ( IsNearToCheckPoint() ) {
			
			if ( !m_bAlreadySetted ) {
				m_fTimeToWait    = m_PatrolPathWaitTiming[ m_PatrolCurrentStep ] + m_tcmTimeManager.TimePassed;
				m_bAlreadySetted = true;
			}
			
			m_enumPatrollingState = DPATROLLINGSTATE.PS_WAITING;
			
			m_PatrolCheckPoint = NextPatrolCheckPoint();
			m_vDirectionToLook = m_PatrolCheckPoint.transform.position - this.transform.position;
			m_vDirectionToLook.Normalize();	
		}
		else {
		}	
	}
	
	private bool IsNearToCheckPoint() {
        Vector3 vA  = m_PatrolCheckPoint.transform.position;
        Vector3 vB  = this.transform.position;
        vA.y        = 0.0f;
        vB.y        = 0.0f;
		return ( vA - vB ).sqrMagnitude < m_DistanceLimit;
	}
	
	public void FixedUpdate() {
		
	}
	
	private void PerformOrientation() {
		
	}

	private GameObject NextPatrolCheckPoint() {

		//if is a loop get the fist one, or the next
		if ( m_Loop ) {
			if ( m_PatrolCurrentStep >= ( m_PatrolPathLenght - 1 ) ) {
				m_PatrolCurrentStep = 0;
			} 
            else {
				m_PatrolCurrentStep++;
			}
		} 
        else {
			//if is not a loop, get the next step, or go backward and get the prevoius
			if ( m_Backward ) {
				if ( m_PatrolCurrentStep <=0 ) {
					m_Backward = false;
				}
			}
			else {
				if ( m_PatrolCurrentStep >= ( m_PatrolPathLenght - 1 ) ) {
					m_Backward = true;
				}
			}
			
			if ( m_Backward ) {
				m_PatrolCurrentStep--;
			}
			else {
				m_PatrolCurrentStep++;
			}
		}
		return m_PatrolPath[ m_PatrolCurrentStep ];
	}
}
