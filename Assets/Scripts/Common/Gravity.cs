using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Gravity : MonoBehaviour
{
    public Transform attractor;
    public Rigidbody rb; // public because attractor script needs it
    public bool on = true; // attract will check to see if this is true or not
    public float Gravitation = -9.8f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
    }

    void Update()
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
        // Get relative rotation to my attractor
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, relativePosition.normalized) * transform.rotation;
        // Slerp to new rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
        // gravity near surface = mg
        rb.AddForce(relativePosition.normalized * rb.mass * Gravitation);
# if DEGUG
        Debug.DrawLine(transform.position, transform.position + relativePosition.normalized * 10);
# endif
    }
}