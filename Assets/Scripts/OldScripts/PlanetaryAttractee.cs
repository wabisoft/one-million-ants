using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlanetaryAttractee : MonoBehaviour
{
    public PlanetaryAttractor attractor;
    public Rigidbody rb; // public because attractor script needs it
    public bool gravityFlag; // attract will check to see if this is true or not

    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        gravityFlag = true;
    }

    void FixedUpdate()
    {
        attractor.Attract(this);
    }
}