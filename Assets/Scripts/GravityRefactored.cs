using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GravityRefactored : MonoBehaviour
{
    public Transform attractor; // massive body
    private Rigidbody rb; // this body
    public bool on = true; // attract will check to see if this is true or not
    public float gravityConst = -40f; // play around with this
    private Vector3 _relPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
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
        // for direction of force
        // this is pointing normal away from sphere
        _relPosition = transform.position - attractor.position;

        // F_g = m*g
        float mass = rb.mass;
        float forceMagnitude = mass * gravityConst;
        Vector3 force = _relPosition.normalized * forceMagnitude;
        rb.AddForce(force, ForceMode.Force); // continuous acceleration with mass
    }
}