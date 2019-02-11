using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragNDrop : MonoBehaviour
{
    public float HeightMultiplier = 1.05f;
    private Camera _camera {get {return Camera.main; }}
    private Rigidbody _rigidbody { get { return GetComponent<Rigidbody>(); } } 
    private Gravity _gravity { get { return GetComponent<Gravity>(); } }
    private Vector3 _up { get { return transform.position - _planet.transform.position; } }
    private Vector3 _axis;
    private float _theta;
    private Planet _planet;

    void OnMouseDown()
    {
        _planet = Utilities.SelectPlanet(gameObject);
        _gravity.on = false;
        transform.position += _up.normalized * HeightMultiplier;
        _rigidbody.velocity = Vector3.zero;
    }

    void OnMouseDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider == GetComponent<Collider>())
            {
                return;
            }
            else {
                var _hitPointRelVec = hit.point - _planet.transform.position;
                _axis = Vector3.Cross(_up, _hitPointRelVec);
                _theta = Vector3.SignedAngle(_up, _hitPointRelVec, _axis);
                transform.RotateAround(_planet.transform.position, _axis, _theta);
            } 
        }
    }

    void OnMouseUp()
    {

        _gravity.on = true;
        var tangentialVelocity = Vector3.Cross(_axis, transform.position - _planet.transform.position);
        _rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, _planet.OrbitalVelocity);
    }

}