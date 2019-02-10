
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GravityRefactored : MonoBehaviour
{
    public Transform attractor;
    public bool RotationLocked = false;
    public bool on = true; // attract will check to see if this is true or not
    public float Gravitation = -9.8f;
    private Rigidbody _rb; 

    void Start()
    {
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
        var relativePosition = (transform.position - attractor.position);
        if (RotationLocked)
        {
            // Get relative rotation to my attractor
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, relativePosition.normalized) * transform.rotation;
            // Slerp to new rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        }
        _rb.AddForce(relativePosition.normalized * _rb.mass * Gravitation, ForceMode.Force);
    }
}