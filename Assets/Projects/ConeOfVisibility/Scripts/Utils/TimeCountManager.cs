using UnityEngine;
using System.Collections;

public class TimeCountManager {

    private float m_fStartTime   			= 0.0f;
    private float m_fPauseTime   			= 0.0f;
    public float TimePassed {
        get { return Time.time - m_fStartTime - m_fPauseTime; }
    }

    public TimeCountManager() {
    }

    public void Start() {
        m_fStartTime = Time.time;
    }

    public void Pause() {
        m_fPauseTime = Time.time;
    }
}
