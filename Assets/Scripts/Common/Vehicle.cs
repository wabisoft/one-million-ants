using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Vehicle : MonoBehaviour
{

    public float MaxSpeed = 3f; // Walking speed
    public float PathRadius = .05f;
    public int PathVertices = 100;
    public bool Steer = true;
    protected Vector3[] _path;
    protected Rigidbody _rigidbody { get { return GetComponent<Rigidbody>();  } }
    protected Planet _planet;
 

    void Start()
    {
        _planet = Utilities.SelectPlanet(gameObject);
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
        _path = new Vector3[PathVertices];
        GeneratePath();
        //PathDelegate(ref _path, _planet.Sphere.center, _planet.Radius);
        // pathDelegate(ref _path, new Vector3(-4.53f, 0.1f, -4.74f), 8.0f);
        DebugPath();
    }

    public void StopSteering()
    {
        Steer = false;
        _rigidbody.velocity = Vector3.zero;
    }
    
    public void StartSteering()
    {
        Steer = true;
    }

    void FixedUpdate()
    { 
#if DEBUG
        for (int i = 1; i < PathVertices; i++)
            Debug.DrawLine(_path[i - 1], _path[i], Color.red);
#endif
        if (!Steer) {
            return;
        }
        if (_rigidbody.velocity == Vector3.zero)
        {
            _rigidbody.velocity += _rigidbody.transform.forward * MaxSpeed / 4.0f;
        }
        Path();
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaxSpeed);
        var targetRotation = Quaternion.FromToRotation(transform.forward, _rigidbody.velocity.normalized) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void Seek(Vector3 target)
    {
        var desire = (transform.position - target).normalized * MaxSpeed;
        var _steering = _rigidbody.velocity - desire;
        _rigidbody.velocity += _steering;
#if DEBUG
        Debug.DrawLine(transform.position, target, Color.black);
        Debug.DrawLine(transform.position, transform.position + _steering * 2, Color.red);
        Debug.DrawLine(transform.position, transform.position + _rigidbody.velocity * 2, Color.blue);
#endif
    }

    public void Arrive(Vector3 target)
    {
        var target_offset = target - transform.position;
        var distance = Vector3.Distance(target, transform.position);
        var ramped_speed = MaxSpeed * (distance / PathRadius);
        var clipped_speed = Mathf.Min(ramped_speed, MaxSpeed);
        var desired_velocity = (clipped_speed / distance) * target_offset;
        var _steering = desired_velocity - _rigidbody.velocity;
        _rigidbody.velocity += _steering;
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
                       \   o = a + (s * normalize(ab)) or a + Project(ap, ab)
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
        // var o = a + Vector3.Project(ap, ab); // TEST TEST TEST
        var s = Vector3.Dot(ap, ab.normalized);
        var o = a + (s * ab.normalized);
        var e = Vector3.Distance(p, o);
        if (e >= PathRadius) {
            if (a == b) {
                Arrive(a);
            }
            else {
                var d = o + ab.normalized;
                Seek(d);
            }
        }
    }

    public void DebugPath()
    {
        for (int i = 1; i < _path.Length; i++)
        {
            Debug.DrawLine(_path[i - 1], _path[i], Color.red);
        }
    }

    protected Vector3[] _pathingSegment(Vector3 point)
    {
        var min = float.MaxValue;
        var index = 0;
        for (var i = 0; i < _path.Length; i++) {
            var dist = (point - _path[i]).sqrMagnitude; //optimization (sqrt is expensive)
            if (dist < min) {
                index = i;
                min = dist;
            }
        }
        if (index + 1 >= _path.Length) {
            return new Vector3[] { _path[index], _path[index] };
        }
        return new Vector3[] { _path[index], _path[index + 1] };
    }

    protected abstract void GeneratePath();
}