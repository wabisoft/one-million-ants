using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragNDrop : MonoBehaviour
{
    public float HeightMultiplier = 1.05f;
    private Camera _camera {get {return Camera.main; }}
    private Rigidbody _rigidbody { get { return GetComponent<Rigidbody>(); } } 
    private Vehicle _vehicle { get { return GetComponent<Vehicle>(); } }
    private Gravity _gravity { get { return GetComponent<Gravity>(); } }
    private Vector3 _up { get { return transform.position - _planet.transform.position; } }
    private Vector3 _axis;
    private float _theta;
    private Planet _planet;
    private Vector3 _previousPos;

    void OnMouseDown()
    {
        _planet = Utilities.SelectPlanet(gameObject);
        _gravity.on = false;
        transform.position += _up.normalized * HeightMultiplier;
        _rigidbody.velocity = Vector3.zero;
        if (_vehicle) {
            // _vehicle.StopSteering();
        }
    }
    
  

    void OnMouseDrag()
    {
        var hit = Utilities.GetPlanetHit(_planet);
        if (hit.HasValue) {
            _previousPos = hit.Value.point;
            var _hitPointRelVec = hit.Value.point - _planet.transform.position;
            _axis = Vector3.Cross(_up, _hitPointRelVec);
            _theta = Vector3.SignedAngle(_up, _hitPointRelVec, _axis);
            transform.RotateAround(_planet.transform.position, _axis, _theta);
        }
    }

    void OnMouseUp()
    {
        _gravity.on = true;
        // var tangentialVelocity = Vector3.Cross(_axis, transform.position - _planet.transform.position);
        var relpos = transform.position - _planet.transform.position;

        var _omega = _theta * _axis.normalized; // really this should be deltaTheta/deltaTime but that didn't give me the behavior I want. \_(?)_/
        var tangentialVelocity = Vector3.Cross(_omega, relpos);
        _rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, _planet.OrbitalVelocity);
    }

}