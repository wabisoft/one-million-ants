using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropable : MonoBehaviour
{
    public SphereCollider sphere;

    private Vector3 _relativeMove;
    private Camera _camera;
    private Rigidbody _rigidbody;

    void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }


    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        _rigidbody.isKinematic = true;
        this.transform.position += this.transform.up * 1.005f;
    }

    void OnMouseDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Vector3 v1 = this.transform.position - sphere.transform.position;
            Vector3 v2 = hit.point - sphere.transform.position;
            _relativeMove = hit.point - this.transform.position;
            Vector3 axis = Vector3.Cross(v1, v2);
            float theta = 1 * Vector3.SignedAngle(v1, v2, axis);
            this.transform.RotateAround(sphere.transform.position, axis, theta);
        }
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {
        this._rigidbody.isKinematic = false;
        this._rigidbody.velocity = Vector3.ClampMagnitude(this._relativeMove / Time.deltaTime, 30);
    }
}
