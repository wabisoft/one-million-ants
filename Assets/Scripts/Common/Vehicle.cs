using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public interface IVehicle
{
    void Seek(Vector3 target);
    void Arrive(Vector3 target);
    void Path();
}

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour, IVehicle
{

    public float MaxSpeed = 3f; // Walking speed
    public Planet Planet;
    public float PathRadius = .05f;
    public int PathVertices = 100;
    public ComputePath PathDelegate = Utilities.ComputeSpiralPath;
    private Vector3[] _path;
    private Rigidbody _rigidbody { get { return GetComponent<Rigidbody>();  } }
    private Vector3 _steering;


    void Start()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
        _path = new Vector3[PathVertices];
        PathDelegate(ref _path, Planet.Sphere.center, Planet.Radius);
        // pathDelegate(ref _path, new Vector3(-4.53f, 0.1f, -4.74f), 8.0f);
    }

    // pass the canvasbounds -- bounds are hard coded
    void FixedUpdate()
    {
#if DEBUG
        for (int i = 1; i < PathVertices; i++)
            Debug.DrawLine(_path[i - 1], _path[i], Color.red);
#endif

        if (_rigidbody.velocity == Vector3.zero)
        {
            _rigidbody.velocity += _rigidbody.transform.forward * MaxSpeed / 4.0f;
        }
        Path();
        _rigidbody.velocity += _steering;
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
    }

    public void Seek(Vector3 target)
    {
        var desire = (transform.position - target).normalized * MaxSpeed;
        _steering = _rigidbody.velocity - desire;
#if DEBUG
        Debug.DrawLine(transform.position, target, Color.black);
        Debug.DrawLine(transform.position, transform.position + _steering * 2, Color.red);
        Debug.DrawLine(transform.position, transform.position + _rigidbody.velocity * 2, Color.blue);
#endif
    }

    public void Arrive(Vector3 target)
    {
        Debug.Log("Arrive");
        var target_offset = target - transform.position;
        var distance = Vector3.Distance(target, transform.position);
        var ramped_speed = MaxSpeed * (distance / PathRadius);
        var clipped_speed = Mathf.Min(ramped_speed, MaxSpeed);
        var desired_velocity = (clipped_speed / distance) * target_offset;
        _steering = desired_velocity - _rigidbody.velocity;
    }

    public void Path()
    {

        // now we have a few relative vectors to work with
        /*
                        p = p0 + v * dt
                       /|
         ap = p - a   / |
                     /  | e = dist(p, o)
                    /   |
                   a----o---->b
                   |----|\     ab = b - a
                       \   o = a + (s * normalize(ab))
                        s = dot(ap, normalize(ab)
        */
        // float dt = Time.deltaTime * dtCoefficient;
        float dt = Time.deltaTime;
        var p = transform.position + _rigidbody.velocity * dt;
        var pathSeg = _pathingSegment(p);
        var a = pathSeg[0];
        var b = pathSeg[1];
        var ab = b - a;
        var ap = p - a;
        var s = Vector3.Dot(ap, ab.normalized);
        var o = a + (s * ab.normalized);
        var e = Vector3.Distance(p, o);
        if (e >= PathRadius)
        {
            if (a == b)
            {
                Arrive(a);
            }
            else
            {
                var d = o + ab.normalized;
                Seek(d);
            }
        }
    }

    private Vector3[] _pathingSegment(Vector3 point)
    {
        var min = float.MaxValue;
        var index = 0;
        for (var i = 0; i < _path.Length; i++)
        {
            var dist = (point - _path[i]).sqrMagnitude; //optimization (sqrt is expensive);
            // var dist = Vector3.Distance(point, _path[i]);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        if (index + 1 >= _path.Length)
        {
            return new Vector3[] { _path[index], _path[index] };
        }
        return new Vector3[] { _path[index], _path[index + 1] };
    }

    void OnDrawGizmos()
    {
        Vector3[] path = new Vector3[PathVertices];
        // pathDelegate(ref path, new Vector3(-4.53f, 0.1f, -4.74f), 8.0f);
        PathDelegate(ref path, Planet.Sphere.center, Planet.Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(path[0], 0.05f);
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Length; i++)
        {
            Gizmos.DrawSphere(path[i], 0.05f);
            Debug.DrawLine(path[i - 1], path[i], Color.red);
        }
    }

}