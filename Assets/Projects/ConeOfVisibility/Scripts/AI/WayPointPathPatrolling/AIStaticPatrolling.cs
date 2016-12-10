using UnityEngine;
using System.Collections;

/*******************************************************
 * Class:           AIStaticPatrolling
 * Description:     Description begin here
 * 
 * Studio Leaves (c) 
 *******************************************************/
public class AIStaticPatrolling : MonoBehaviour {
	
    public enum PATROLLINGDIRECTION {
          D_LOOKAT_PZ
        , D_LOOKAT_NZ
        , D_LOOKAT_PX
        , D_LOOKAT_NX

		, D_2D_LOOKAT_DOWN
		, D_2D_LOOKAT_UP
		, D_2D_LOOKAT_RIGHT
		, D_2D_LOOKAT_LEFT
    }

    public enum STATICPATROLLINGSTATE {
          PS_ROTATING
        , PS_WAITING
    }

    public  PATROLLINGDIRECTION[] 	m_DirectionsToLook;
    public  float[]     	        m_WaitingTime;
    public  float			        m_Speed				    = 10.0f;
    public  bool 	                m_IsContinuous			= false;
	public  bool 					m_bIs2D 				= false;

    private float 			        m_fTimeToWait;
    private int 			        m_iCurrentPosition  	= 0;
    private bool 			        m_bClockWise         	= true;
    private bool 			        m_bAlreadySetted    	= false;
    private bool 			        m_bRotateOnce 	  	    = false; //Se il nostro nemico rimarrà fermo in una posizione
    private bool 			        m_bRotateAlready		= false;
    private bool                    m_bStartPatrolling      = true;
    private Vector3                 m_vDirectionToLook      = new Vector3( 0.0f, 0.0f, 0.0f );
    private PATROLLINGDIRECTION     m_enDirectionToLook;
    private STATICPATROLLINGSTATE   m_enPatrollingState;

    public bool ActivePatrolling {
        set { m_bStartPatrolling = value; }
        get { return m_bStartPatrolling; }
    }

    public Vector3 DirectionToLook {
        get { return m_vDirectionToLook; }
    }

    public PATROLLINGDIRECTION DirectionIDToLook {
        get { return m_enDirectionToLook; }
    }

    public STATICPATROLLINGSTATE PatrollingState {
        get { return m_enPatrollingState; }
    }

    void Start() {
        InitAIPatrolling();

		/* Check for 2D */
		AIConeDetection AICone = this.GetComponent<AIConeDetection>();
		if ( AICone != null ) {
			m_bIs2D = AICone.Is2DCone;
		}
    }

    void Update() {
        if ( m_bStartPatrolling ) {
			if ( m_IsContinuous ) {
				PerformContinuesPatrolling();
			} 
			else {
				PerformPatrolling();
			}
        }
    }

    private void InitAIPatrolling() {
        if ( m_DirectionsToLook.Length != m_WaitingTime.Length ) {
            Debug.LogError( "DirectionsToLook and WaitingTime must have the same lenght!" );
            Debug.Break();
        }

        SetNextPointToLook();   
        m_bAlreadySetted = false;
   
        if ( m_DirectionsToLook.Length == 1 )
            m_bRotateOnce = true;
    }

    public void StartTimeCounting() {
        m_fTimeToWait = Time.time + m_WaitingTime[ m_iCurrentPosition ];
    }

    private void PerformContinuesPatrolling() {

        if ( !m_bAlreadySetted && !IsNextPositionReached( 1.0f ) ) {
			
			if ( m_bClockWise ) {
				if ( !m_bIs2D ) {
					this.transform.Rotate( Vector3.up * Time.deltaTime * m_Speed );
				} 
				else {
					this.transform.Rotate( Vector3.left * Time.deltaTime * m_Speed );
				}
			} 
			else {
				if ( !m_bIs2D ) {
					this.transform.Rotate( -Vector3.up * Time.deltaTime * m_Speed );
				} 
				else {
					this.transform.Rotate( -Vector3.left * Time.deltaTime * m_Speed );
				}
			}
        }
        else {
            if ( !m_bAlreadySetted ) {
                m_bAlreadySetted    = true;
                m_fTimeToWait       = Time.time + m_WaitingTime[ m_iCurrentPosition ];
                m_enPatrollingState = STATICPATROLLINGSTATE.PS_WAITING;
            }

            if ( Time.time > m_fTimeToWait && m_bAlreadySetted ) {
                m_enPatrollingState = STATICPATROLLINGSTATE.PS_ROTATING;
                UpdateCountPosition();
                SetNextPointToLook();
                m_bAlreadySetted    = false;

                UpdateClockwiseRotation();
            }
        }
    }

    private bool m_bAlreadyReached = false;
    private void PerformPatrolling() {
		
        if ( !m_bAlreadyReached && Time.time > m_fTimeToWait && IsNextPositionReached( 1.0f ) ) {
            m_bAlreadyReached   = true;
            m_fTimeToWait       = Time.time + m_WaitingTime[ m_iCurrentPosition ];

            if ( m_bRotateOnce )
                m_bRotateAlready = true;

            m_enPatrollingState = STATICPATROLLINGSTATE.PS_WAITING;
        }

        if ( !m_bRotateAlready && m_bAlreadyReached && Time.time > m_fTimeToWait ) {
            UpdateCountPosition();
            SetNextPointToLook();
            m_bAlreadyReached   = false;
            m_enPatrollingState = STATICPATROLLINGSTATE.PS_ROTATING;
        }

        PerformNotContinuesRotation();
    }

    private void UpdateClockwiseRotation() {

        //Debug.Log( "Forward: " + this.transform.forward + " DirToLook: " + m_vDirectionToLook );
        if ( m_vDirectionToLook == Vector3.back ) {
            if ( this.transform.forward.x > 0.0f ) {
                m_bClockWise = true;
            }
            else {
                m_bClockWise = false;
            }
        }
        else if ( m_vDirectionToLook == Vector3.forward ) {
            if ( this.transform.forward.x > 0.0f ) {
                m_bClockWise = false;
            }
            else {
                m_bClockWise = true;
            }
        }
        else if ( m_vDirectionToLook == Vector3.left ) {

			if ( !m_bIs2D ) {
				if ( this.transform.forward.z > 0.0f ) {
					m_bClockWise = false;
				}
				else {
					m_bClockWise = true;
				}
			} 
			else {
				if ( this.transform.forward.y > 0.0f ) {
					m_bClockWise = true;
				}
				else {
					m_bClockWise = false;
				}
			}
        }
        else if ( m_vDirectionToLook == Vector3.right ) {

			if ( !m_bIs2D ) {
				if ( this.transform.forward.z > 0.0f ) {
					m_bClockWise = true;
				}
				else {
					m_bClockWise = false;
				}
			} 
			else {
				if ( this.transform.forward.y > 0.0f ) {
					m_bClockWise = false;
				}
				else {
					m_bClockWise = true;
				}
			}
        }
		/* Nel 2D si ragiona al contrario */
		else if ( m_vDirectionToLook == Vector3.down ) {
			if ( this.transform.forward.x > 0.0f ) {
				m_bClockWise = false;
			}
			else {
				m_bClockWise = true;
			}
		}
		else if ( m_vDirectionToLook == Vector3.up ) {
			if ( this.transform.forward.x > 0.0f ) {
				m_bClockWise = true;
			}
			else {
				m_bClockWise = false;
			}
		}

    }

    private bool IsNextPositionReached( float iAngle ) {

        Vector3 dirA = this.transform.forward;
		Vector3 dirB = m_vDirectionToLook;

		if ( !m_bIs2D ) {
			dirA.y = 0.0f;
			dirB.y = 0.0f;
		} 
		else {
			dirA.z = 0.0f;
			dirB.z = 0.0f;
		}

        float Angle = Vector3.Angle( dirA, dirB );

		if ( Angle < iAngle ) {
			return true;
		}

        return false;
    }

    Vector3 m_vForward = new Vector3( 0.0f, 0.0f, 0.0f );
    private void PerformNotContinuesRotation() {
        m_vForward = this.transform.forward;

		if ( !m_bIs2D ) {
			m_vForward.x = Mathf.Lerp( m_vForward.x, m_vDirectionToLook.x, Time.deltaTime * m_Speed );
			m_vForward.z = Mathf.Lerp( m_vForward.z, m_vDirectionToLook.z, Time.deltaTime * m_Speed );
		} 
		else {
			m_vForward.x = Mathf.Lerp( m_vForward.x, m_vDirectionToLook.x, Time.deltaTime * m_Speed );
			m_vForward.y = Mathf.Lerp( m_vForward.y, m_vDirectionToLook.y, Time.deltaTime * m_Speed );
		}

        this.transform.forward = m_vForward;
    }

    private void UpdateCountPosition() {
        ++m_iCurrentPosition;
        if ( m_iCurrentPosition == m_DirectionsToLook.Length ) {
            m_iCurrentPosition = 0;
        }
    }

    private void SetNextPointToLook() {
        if ( m_DirectionsToLook.Length > 0 ) {
            switch ( m_DirectionsToLook[ m_iCurrentPosition ] ) {
            
				case PATROLLINGDIRECTION.D_LOOKAT_PZ:
                 	m_vDirectionToLook = Vector3.forward;
                    m_enDirectionToLook = PATROLLINGDIRECTION.D_LOOKAT_PZ;
                    break;
            	case PATROLLINGDIRECTION.D_LOOKAT_NZ:
                    m_vDirectionToLook = Vector3.back;
                    m_enDirectionToLook = PATROLLINGDIRECTION.D_LOOKAT_NZ;
                    break;
            	case PATROLLINGDIRECTION.D_LOOKAT_NX:
                    m_vDirectionToLook = Vector3.left;
                    m_enDirectionToLook = PATROLLINGDIRECTION.D_LOOKAT_NX;
                    break;
            	case PATROLLINGDIRECTION.D_LOOKAT_PX:
                    m_vDirectionToLook = Vector3.right;
                    m_enDirectionToLook = PATROLLINGDIRECTION.D_LOOKAT_PX;
                    break;
				case PATROLLINGDIRECTION.D_2D_LOOKAT_DOWN:
					m_vDirectionToLook = Vector3.down;
					m_enDirectionToLook = PATROLLINGDIRECTION.D_2D_LOOKAT_DOWN;
					break;
				case PATROLLINGDIRECTION.D_2D_LOOKAT_RIGHT:
					m_vDirectionToLook = Vector3.right;
					m_enDirectionToLook = PATROLLINGDIRECTION.D_2D_LOOKAT_RIGHT;
					break;
				case PATROLLINGDIRECTION.D_2D_LOOKAT_LEFT:
					m_vDirectionToLook = Vector3.left;
					m_enDirectionToLook = PATROLLINGDIRECTION.D_2D_LOOKAT_LEFT;
					break;
				case PATROLLINGDIRECTION.D_2D_LOOKAT_UP:
					m_vDirectionToLook = Vector3.up;
					m_enDirectionToLook = PATROLLINGDIRECTION.D_2D_LOOKAT_UP;
					break;
            }
        }
    }
}
