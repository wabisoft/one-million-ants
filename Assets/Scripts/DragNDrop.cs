using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public SphereCollider sphere;
    private Camera _camera;
    private Rigidbody _rigidbody; 
    private GravityRefactored _gravity;
    private Vector3 _prevPos, _upDir, _axis, _hitPointRelVec, _tangentialVel;
    private float _heightMultiplier, _theta;
    private float _angularVel;

    void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _gravity = GetComponent<GravityRefactored>();
        _heightMultiplier = 1.0f;
    }

    void OnMouseDown()
    {
        _gravity.on = false;
        transform.position += _upDir.normalized * _heightMultiplier;
        _rigidbody.velocity = Vector3.zero;
    }

    void OnMouseDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            _upDir = transform.position - sphere.transform.position;
            _hitPointRelVec = hit.point - sphere.transform.position;
            _axis = Vector3.Cross(_upDir, _hitPointRelVec);
            _theta = Vector3.SignedAngle(_upDir, _hitPointRelVec, _axis);

            // needed for accurate physics, but it's too much for game
            _angularVel = _theta/Time.deltaTime;

            // looks ok...
            // Debug.DrawLine(_prevPos, transform.position, Color.red, 2.5f);

            // tangential direction
            _tangentialVel = transform.position - _prevPos;
        }
    }
    void OnMouseUp()
    {
        _gravity.on = true;
        // v_tangential = angularVel * radius
        Vector3 tangentialVelocity = 0.1f * (_tangentialVel.normalized) * _angularVel * sphere.radius;
        // Debug.Log(_rigidbody.velocity.ToString());
        // was breaking at around 22;
        _rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, 15f);
    }
    void FixedUpdate()
    {
        _prevPos = transform.position;

        if(_gravity.on == false)
        {
            transform.RotateAround(sphere.transform.position, _axis, _theta);
        }
    }
}