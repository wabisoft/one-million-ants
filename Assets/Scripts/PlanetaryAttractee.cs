using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlanetaryAttractee : MonoBehaviour
{
    public PlanetaryAttractor attractor;
    public Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidBody.useGravity = false;
    }

    void Update()
    {
        attractor.Attract(this);
    }
}