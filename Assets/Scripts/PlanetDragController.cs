using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDragController : MonoBehaviour
{

    private Camera _camera;
    private Vector3? _previousPoint;
    // Use this for initialization
    void Start()
    {
        _camera = Camera.main;
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
            Vector3 axis = Vector3.Cross(v1, v2);
            float theta = -1 * Vector3.SignedAngle(v1, v2, axis);
            _camera.transform.RotateAround(this.transform.position, axis, theta);
        }
    }

    void OnMouseUp()
    {
        _previousPoint = null;

    }

}
