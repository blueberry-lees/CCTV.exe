using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayTimeTracker : MonoBehaviour
{
    public static PlayTimeTracker Instance;

    private float sessionStartTime;
    private bool isTracking = false;

    public float SessionElapsedTime => Time.time - sessionStartTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartTracking()
    {
        sessionStartTime = Time.time;
        isTracking = true;
    }

    public float StopTracking()
    {
        if (!isTracking) return 0f;
        float elapsed = Time.time - sessionStartTime;
        isTracking = false;
        return elapsed;
    }
}
