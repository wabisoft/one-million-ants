using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDragController : MonoBehaviour
{

    private Camera _camera;
    private Vector3? _previousPoint;
    private Vector3 _axis;
    private float _angularVel;
    private float _theta;
    private float _spinTimer;
 
    void Start()
    {
        _camera = Camera.main;
        _spinTimer = 0.0f;
    }

    void Update()
    {
        if( _axis.magnitude != 0)
        {
            float deltaTheta = _angularVel * Time.deltaTime;
            deltaTheta *= Mathf.Exp(-1.5f * _spinTimer);
            _camera.transform.RotateAround(this.transform.position, _axis, deltaTheta);
        }

        _spinTimer += Time.deltaTime;
    }

    public void OnMouseDrag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3? currentPoint = null;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (_previousPoint == null)
            {
                _previousPoint = hit.point;
                return;
            }
            else currentPoint = hit.point;
            Vector3 v1 = _previousPoint.Value - this.transform.position;
            Vector3 v2 = currentPoint.Value - this.transform.position;
            _axis = Vector3.Cross(v1, v2);
            _theta = -1 * Vector3.SignedAngle(v1, v2, _axis);

            _angularVel = _theta/Time.deltaTime;
            _camera.transform.RotateAround(this.transform.position, _axis, _theta);
        }
    }

    void OnMouseUp()
    {
        _previousPoint = null;
        _spinTimer = 0.0f;
    }
}
