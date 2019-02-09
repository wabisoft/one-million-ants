using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just testing
// this behavior should probably be in a UI element if it's fun

public class zoomTest : MonoBehaviour
{
    private Camera _cam;
    // private bool _easingSwitch;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        // _easingSwitch = true;
    }

    // Update is called once per frame
    void Update()
    {
        // right click
        if (Input.GetMouseButtonDown(1))
        {
            // float targetFieldOfView = _cam.fieldOfView * 0.75f;
            // float diff = _cam.fieldOfView - targetFieldOfView;
            _cam.fieldOfView *= .75f;
        }
        if (Input.GetMouseButtonDown(2))
        {
            _cam.fieldOfView *= 1.33f;
        }
    }
}
