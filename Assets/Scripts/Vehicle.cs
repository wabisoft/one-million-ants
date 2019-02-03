
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Vehical : MonoBehaviour
{

    public float MaxSpeed = 3f; // Walking speed
    public SphereCollider Planet;
    public float pathRadius = .05f;
    public int pathVertices = 100;
    public ComputePath pathDelegate = Utilities.ComputeSpiralPath;
    private Vector3[] _path;
    private Rigidbody _rigidbody;
    private Vector3 _steering;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
        _path = new Vector3[pathVertices];
        this.pathDelegate(ref _path, Planet.center, Planet.radius * Planet.transform.localScale.x);
        // this.pathDelegate(ref _path, new Vector3(-4.53f, 0.1f, -4.74f), 8.0f);
    }

    // pass the canvasbounds -- bounds are hard coded
    void FixedUpdate()
    {
        for (int i = 1; i < pathVertices; i++)
            Debug.DrawLine(_path[i - 1], _path[i], Color.red);

        if (this._rigidbody.velocity == Vector3.zero)
        {
            this._rigidbody.velocity += this._rigidbody.transform.forward * MaxSpeed / 4.0f;
        }
        path();
        this._rigidbody.velocity += _steering;
        this._rigidbody.velocity = Vector3.ClampMagnitude(this._rigidbody.velocity, this.MaxSpeed);
    }

    void seek(Vector3 target)
    {
        var desire = (this.transform.position - target).normalized * this.MaxSpeed; //* Time.deltaTime;
        _steering = this._rigidbody.velocity - desire;
        // Vector3.ClampMagnitude(
#if DEBUG
        Debug.DrawLine(this.transform.position, target, Color.black);
        Debug.DrawLine(this.transform.position, this.transform.position + _steering * 2, Color.red);
        Debug.DrawLine(this.transform.position, this.transform.position + this._rigidbody.velocity * 2, Color.blue);
#endif
    }

    void arrive(Vector3 target)
    {
        Debug.Log("Arrive");
        var target_offset = target - this.transform.position;
        var distance = Vector3.Distance(target, this.transform.position);
        var ramped_speed = MaxSpeed * (distance / pathRadius);
        var clipped_speed = Mathf.Min(ramped_speed, MaxSpeed);
        var desired_velocity = (clipped_speed / distance) * target_offset;
        _steering = desired_velocity - this._rigidbody.velocity;
    }

    void path()
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
        var p = this.transform.position + this._rigidbody.velocity * dt;
        var pathSeg = pathingSegment(p);
        var a = pathSeg[0];
        var b = pathSeg[1];
        var ab = b - a;
        var ap = p - a;
        var s = Vector3.Dot(ap, ab.normalized);
        var o = a + (s * ab.normalized);
        var e = Vector3.Distance(p, o);
        if (e >= pathRadius)
        {
            if (a == b)
            {
                arrive(a);
            }
            else
            {
                var d = o + ab.normalized;
                seek(d);
            }
        }
    }

    private Vector3[] pathingSegment(Vector3 point)
    {
        var min = float.MaxValue;
        var index = 0;
        for (var i = 0; i < _path.Length; i++)
        {
            var dist = Vector3.Distance(point, _path[i]);
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
        Vector3[] path = new Vector3[pathVertices];
        // this.pathDelegate(ref path, new Vector3(-4.53f, 0.1f, -4.74f), 8.0f);
        this.pathDelegate(ref path, Planet.center, Planet.radius * Planet.transform.localScale.x);
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