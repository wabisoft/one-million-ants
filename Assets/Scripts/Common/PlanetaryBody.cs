using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetaryBody : MonoBehaviour
{
    public bool RotationLocked;
    public Rigidbody Rigidbody { get { return GetComponent<Rigidbody>();  } }
    private Planet _planet;
    public Planet Planet {
        get {
            if (!_planet) {
                _planet = Utilities.SelectPlanet(gameObject);
            }
            return _planet;
        }
        set {
            _planet = value;
        }
    }

    public Vector3 RelativeToPlanet
    {
        get {
            return transform.position - Planet.transform.position;
        }
    }

    public Vector3 Up {
        get {
            return RelativeToPlanet.normalized;
        }
    }

    public Vector3 Down{
        get {
            return -Up;
        }
    }    
   
    public bool Grounded()
    {
        var sqrDistToPlanet = (transform.position - Planet.transform.position).sqrMagnitude;
        var sqrGroundedDist = Mathf.Pow(Planet.Radius + transform.localScale.y * 1.6f, 2); 
        if (sqrDistToPlanet > sqrGroundedDist) {
            return false;
        } else {
            return true;
        }
    }

    public void StayGrounded()
    {
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);
    }

    public void StayUp()
    {
        // Get relative rotation to planet
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Up) * transform.rotation;
        // Slerp to new rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void Gravitate()
    {
        if (RotationLocked) {
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        var down = Down; // cache up to avoid extra square routes

        if (RotationLocked)
        {
            StayUp();
        }
        // var force_mag = (RelativeToPlanet.magnitude - Planet.Radius) / (int)SteeringBehavior.Deceleration.fast * 0.3f;
#if DEBUG
        Debug.DrawLine(transform.position, transform.position + down * 2, Color.black);
#endif
        var force = down * Rigidbody.mass * Globals.Gravitation;
        Rigidbody.AddForce(force, ForceMode.Force);
    }

}