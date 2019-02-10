using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public SphereCollider sphere;
    public float HeightMultiplier = 1.0f;
    private Camera _camera;
    private Rigidbody _rigidbody; 
    private GravityRefactored _gravity;
    private Vector3 _upDir { get { return transform.position - sphere.transform.position; } }
    private Vector3 _axis;
    private float _sphereRadius { get { return sphere.radius * sphere.transform.localScale.x; } }
    private float _theta;
    private bool _doUpdate = false;

    void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _gravity = GetComponent<GravityRefactored>();
    }

    void OnMouseDown()
    {
        _gravity.on = false;
        _doUpdate = true;
        transform.position += _upDir.normalized * HeightMultiplier;
        _rigidbody.velocity = Vector3.zero;
        //_rigidbody.angularVelocity = Vector3.zero;
    }

    void OnMouseDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider == GetComponent<Collider>())
            {
                _doUpdate = false;
                return;
            }
            else {
                _doUpdate = true;
                //_upDir = transform.position - sphere.transform.position;
                var _hitPointRelVec = hit.point - sphere.transform.position;
                _axis = Vector3.Cross(_upDir, _hitPointRelVec);
                _theta = Vector3.SignedAngle(_upDir, _hitPointRelVec, _axis);
                // needed for accurate physics, but it's too much for game
            } 
        }
        else
        {
            _doUpdate = false;
        }  
    }
    void OnMouseUp()
    {

        _gravity.on = true;
        _doUpdate = false;
        var tangentialVelocity = Vector3.Cross(_axis, transform.position - sphere.transform.position);
        var orbitalVelocity = Mathf.Sqrt(Mathf.Abs(_gravity.Gravitation * 60 / _sphereRadius));
        _rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, orbitalVelocity);
    }

    void FixedUpdate()
    {
        //_prevPos = transform.position;
        if(_doUpdate)
        {
            transform.RotateAround(sphere.transform.position, _axis, _theta);
        }
    }
}