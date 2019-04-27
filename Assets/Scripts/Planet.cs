using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public event Action OnClicked;

    public PlanetDragController Drag { get { return GetComponent<PlanetDragController>(); } }
    public SphereCollider Sphere { get { return GetComponent<SphereCollider>(); } }
    public float Radius {
        get {
            return Sphere.radius * Sphere.transform.localScale.x;
        }
    }
    public const float Gravity = 40;
    public float Mass = 60f;
    public float OrbitalVelocity { get; private set; }

    private void Start()
    {
        OrbitalVelocity = Mathf.Sqrt(Mathf.Abs(Globals.Gravitation * Mass / Radius)); // cache this to avoid using sqrt every fixed update
    }

    public Vector3 Random()
    {
        return UnityEngine.Random.onUnitSphere * Radius;
    }

    public void OnMouseDown()
    {
        if (OnClicked != null)
            OnClicked();
    }

}
