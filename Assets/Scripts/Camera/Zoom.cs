using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just testing
// this behavior should probably be in a UI element if it's fun

public class Zoom : MonoBehaviour
{
    private Camera _cam;
    private float _FOVDifference;
    private float _startFOV;
    private float timeStep;
    private float val;

    void Start()
    {
        _cam = Camera.main;
        timeStep = 0.0f;
        val = 0.0f;
    }

    public void zoomIn()
    {
        _FOVDifference = _cam.fieldOfView - 0.75f * _cam.fieldOfView;
        _startFOV = _cam.fieldOfView;

        while(timeStep < 1)
        {
            val = _FOVDifference * Mathf.Sqrt(timeStep);
            _cam.fieldOfView = _startFOV - val;
            // timeStep += 0.02f;
            timeStep += Time.deltaTime;
        }

        timeStep = 0.0f;
    }

    public void zoomOut()
    {
        _FOVDifference = 1.33f * _cam.fieldOfView - _cam.fieldOfView;
        _startFOV = _cam.fieldOfView;
        
        while(timeStep < 1)
        {
            val = _FOVDifference * Mathf.Sqrt(timeStep);
            _cam.fieldOfView = _startFOV + val;
            // timeStep += 0.02f;
            timeStep += Time.deltaTime;
        }

        timeStep = 0.0f;
    }
}
