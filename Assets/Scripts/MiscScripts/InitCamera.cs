using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCamera : MonoBehaviour
{
    // We should use an easing function to zoom in at start of game:
    // y = x^5

    // y\ =\ \sin\left(1.65x\right)^2\ \cdot\ x

    // We can show a crash and a parachute somewhere else.
    // this will explain what to do without any needed directions a'la mario level 0
    private Camera cam;
    private bool _easingSwitch = false;
    private int _FOVDifference;
    public int startFOV;
    public int endFOV;
    private float timer;

    void Start()
    {
        _easingSwitch = true;
        cam = gameObject.GetComponent<Camera>();
        cam.fieldOfView = startFOV;
        _FOVDifference = startFOV - endFOV;
    }

    void Update()
    {
        
        if(_easingSwitch)
        {
            if(cam.fieldOfView > endFOV)
            {
                // 0. linear shaping
                // cam.fieldOfView -= _FOVDifference * timer;

                // 1. basic quadratic
                // cam.fieldOfView -= _FOVDifference * Mathf.Pow(timer, 2);

                // 2. basic cubic
                // cam.fieldOfView -= _FOVDifference * Mathf.Pow(timer, 3);
                
                // 3. just playing around with different shaping functions
                cam.fieldOfView -= _FOVDifference * Mathf.Pow(Mathf.Sin(1.65f * timer), 2) * timer;
            }
            else
            {
                _easingSwitch = false;
                timer = 0f;
            }
            timer += 0.007f;
        }
        
    }
}
