
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    public bool RotationLocked = false;
    public bool on = true; // attract will check to see if this is true or not
    private Rigidbody _rb;
    private Planet _planet;

    void Start()
    {
         
        _planet = Utilities.SelectPlanet(gameObject);
        _rb = GetComponent<Rigidbody>();
        if (RotationLocked)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        _rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (on)
        {
            Gravitate();
        }
    }

    void Gravitate()
    {
        // relative position between me and my attractor
        var relativePosition = (transform.position - _planet.transform.position);
        if (RotationLocked)
        {
            // Get relative rotation to my attractor
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, relativePosition.normalized) * transform.rotation;
            // Slerp to new rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        }
        _rb.AddForce(relativePosition.normalized * _rb.mass * Globals.Gravitation, ForceMode.Force);
    }

    
}