using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragNDrop : PlanetaryBody
{
    // TODO: Make me a state in an FSM

    public float HeightMultiplier = 1.05f;
    private Rigidbody _rigidbody { get { return GetComponent<Rigidbody>(); } } 
    private Vector3 _axis;
    private float _theta;
    
    private Vector3 _previousPos;

    //private void FixedUpdate()
    //{
    //    if ((transform.position - Planet.Sphere.transform.position).sqrMagnitude <= Mathf.Pow(Planet.Radius, 2) * 1.01f) {
    //        _rigidbody.isKinematic = true;
    //    } else {
    //        _rigidbody.isKinematic = false;
    //    }
    //}

    void OnMouseDown()
    {
        transform.position += Up * HeightMultiplier;
        _rigidbody.velocity = Vector3.zero;
    }  

    void OnMouseDrag()
    {
        var hit = Utilities.GetPlanetHit(Planet);
        if (hit.HasValue) {
            _previousPos = hit.Value.point;
            var _hitPointRelVec = hit.Value.point - Planet.transform.position;
            _axis = Vector3.Cross(Up, _hitPointRelVec);
            _theta = Vector3.SignedAngle(Up, _hitPointRelVec, _axis);
            transform.RotateAround(Planet.transform.position, _axis, _theta);
        }
    }

    void OnMouseUp()
    {
        // var tangentialVelocity = Vector3.Cross(_axis, transform.position - Planet.transform.position);
        var relpos = transform.position - Planet.transform.position;

        var _omega = _theta * _axis.normalized; // really this should be deltaTheta/deltaTime but that didn't give me the behavior I want. \_(?)_/
        var tangentialVelocity = Vector3.Cross(_omega, relpos);
        _rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, Planet.OrbitalVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.02f);
    }
}