using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Vehicle : MonoBehaviour
{

    public float MaxSpeed = 3f; // Walking speed
    public float PathRadius = .05f;
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
    
    public Vector3 Velocity
    {
        get { return Rigidbody.velocity;  }
        protected set { Rigidbody.velocity = value;  }
    }

    public Vector3 Position 
    {
        get { return transform.position; }
        //set { transform.position = value; }
    }

    private SteeringBehavior _steering;
    public SteeringBehavior Steering
    {
        get {
            if (_steering == null) {
                _steering = new SteeringBehavior(this);
            }
            return _steering;
        }
    }

    public void FaceFront()
    {
        var targetRotation = Quaternion.FromToRotation(transform.forward, Velocity.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void ClampSpeed()
    {
        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, MaxSpeed);
    }

    public bool Grounded()
    {
        var sqrDistToPlanet = (Position - Planet.transform.position).sqrMagnitude;
        var sqrGroundedDist = Mathf.Pow(Planet.Radius + transform.localScale.y * 1.6f, 2); 
        if (sqrDistToPlanet > sqrGroundedDist) {
            return false;
        } else {
            return true;
        }
    }

    protected abstract void GeneratePath();
}