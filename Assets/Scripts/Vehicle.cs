using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Vehicle : PlanetaryBody 
{

    public float PathRadius = .6f;
    public float ArriveRadius = 0.4f;
    public float SquaredPathRadius { 
        get {
            return Mathf.Pow(PathRadius, 2);
        }
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

    public void FaceRight()
    {
        var targetRotation = Quaternion.FromToRotation(transform.right, Velocity.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void FaceLeft()
    {
        var targetRotation = Quaternion.FromToRotation(-1 * transform.right, Velocity.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void FaceBack()
    {
        var targetRotation = Quaternion.FromToRotation(-1 * transform.forward, Velocity.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}