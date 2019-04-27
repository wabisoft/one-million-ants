using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetaryBody : MonoBehaviour
{

    public virtual event Action<PlanetaryBody> OnClicked;

    public float MaxSpeed = 3f; // Walking speed

    public Stack<PlanetaryBodyMotion> Motions;
    public bool RotationLocked; 

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody {
        get {
            if (! rigidbody)
                rigidbody = GetComponent<Rigidbody>();
            return rigidbody;
        }
    }

    public Vector3 Velocity
    {
        get { return Rigidbody.velocity;  }
        protected set { Rigidbody.velocity = value;  }
    }


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

    public Vector3 OnPlanetPosition
    {
        get {
            return RelativeToPlanet.normalized * Planet.Radius + Planet.transform.position;
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

    public void Gravitate(float gravitation = Globals.Gravitation)
    {
        if (RotationLocked) {
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (RotationLocked)
        {
            StayUp();
        }
#if DEBUG
        Debug.DrawLine(transform.position, transform.position + Down * 2, Color.black);
#endif
        var force = Down * Rigidbody.mass * gravitation;
        Rigidbody.AddForce(force, ForceMode.Force);
        
    }

    public void Drag()
    {
        var rho = 1.2f;
        var dragCoef = 0.5f;
        var dragForceCoef = 0.5f * rho * dragCoef;
        var dragForce = Velocity * -dragForceCoef;
        Rigidbody.AddForce(dragForce, ForceMode.Force);
    }

    public void ClampSpeed(float? speed = null)
    {
        if (speed.HasValue) {
            Velocity = Vector3.ClampMagnitude(Velocity, speed.Value);
        } else {
            Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
        }
    }

    public void DebugVelocity(int timeStep = 2)
    {
        Debug.DrawLine(transform.position, transform.position + Velocity * timeStep, Color.magenta); 
    }

    public virtual void Start()
    {
        Motions = new Stack<PlanetaryBodyMotion>();
        Motions.Push(PlanetaryBodyMotions.Standing);
        Rigidbody.useGravity = false;
    } 

    public virtual void OnEnable()
    {
        Planet.OnClicked += OnPlanetClicked;
    }

    public virtual void OnDisable()
    {
        Planet.OnClicked -= OnPlanetClicked;
    }

    public virtual void FixedUpdate()
    {
        Motions.Peek().FixedUpdate(this);
    }

    public virtual void Update()
    {
        Motions.Peek().Update(this);
    }

    public virtual void OnMouseDown()
    {
        // Emit OnClicked for this PlanetaryBody
        if (OnClicked != null)
            OnClicked(this);
        // Let the state try and do stuff too.
        Motions.Peek().OnMouseDown(this);
    }

    public virtual void OnMouseDrag()
    {
        Motions.Peek().OnMouseDrag(this);
    }

    public virtual void OnMouseUp()
    {
        Motions.Peek().OnMouseUp(this);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Motions.Peek().OnCollisionEnter(this, collision);
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        Motions.Peek().OnCollisionStay(this, collision);
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        Motions.Peek().OnCollisionExit(this, collision);
    }

    public virtual void OnPlanetClicked()
    {
        Motions.Peek().OnPlanetClicked(this);
    }
}