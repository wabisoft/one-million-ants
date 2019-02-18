//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DragDropable : MonoBehaviour
//{
//    public SphereCollider sphere;

//    // private Vector3 relativeMove;
//    private Vector3 _tangentialVelocity, _normal, _height;
//    private Camera _camera;
//    private Rigidbody _rigidbody; // 
//    private Gravity _gravity;


//    public float heightMultiplier;
//    void Start()
//    {
//        _camera = Camera.main;
//        _rigidbody = GetComponent<Rigidbody>();
//        _gravity = GetComponent<Gravity>();
//        heightMultiplier = 0.8f;
//        _height = this.transform.up * heightMultiplier;
//    }
//    /// <summary>
//    /// OnMouseDown is called when the user has pressed the mouse button while
//    /// over the GUIElement or Collider.
//    /// </summary>
//    void OnMouseDown()
//    {
//        // turn off forces -- note that our faux Gravity isn't recognized automatically
//        // and has to be manually accounted for
//        // I'm deciding to not use isKinematic to save collision detection
//        _rigidbody.velocity = Vector3.zero;
//        //_gravity.on = false;

//        this.transform.position += _height; // Height is reused to keep tangential velocity consistent,  SEE _relativeMove operation
//    }

//    void OnMouseDrag()
//    {
//        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, 1000))
//        {
//            Vector3 v1 = this.transform.position - sphere.transform.position;
//            Vector3 v2 = hit.point - sphere.transform.position;
//            Vector3 _normal = v2.normalized;
//            // (hit.point + _height) - this.transform.position; always going to be a differential distance
//            Vector3 relativeMove = (hit.point + _height) - this.transform.position; // needs to be same for tangential velocity
//            _tangentialVelocity = relativeMove / Time.deltaTime;

//            // Debug.Log(_tangentialVelocity);
//            Vector3 axis = Vector3.Cross(v1, v2);
//            float theta = Vector3.SignedAngle(v1, v2, axis);
//            this.transform.RotateAround(sphere.transform.position, axis, theta);

//            // Naive solution/ brute force
//            _rigidbody.velocity = Vector3.zero; // account for losing isKinematic properties
//        }
//    }

//    /// <summary>
//    /// OnMouseUp is called when the user has released the mouse button.
//    /// </summary>
//    void OnMouseUp()
//    {

//        //_gravity.on = true;
//        // _rigidbody.velocity = _tangentialVelocity;
//        // // centripetal force F_c = m (v^2/r)
//        // float centripetalCoefficient = _rigidbody.mass * Mathf.Pow(_tangentialVelocity.magnitude, 2)/sphere.radius;
//        // _rigidbody.AddForce(-1 * _normal * centripetalCoefficient);
//    }
//}
