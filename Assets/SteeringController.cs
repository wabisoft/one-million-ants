
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SteeringController : MonoBehaviour
{
    public Transform pointer;
    public Transform closest;
    public Transform secondClosest;
    public float MaxSpeed = 1.4f; // Walking speed
    public float MaxAcceleration = 0.25f; // These seems to be a good cap for the above speed
    public SphereCollider Planet;
    // public float radius = 4f;
    public float fleeRadius = 4f;
    public float pathRadius = .5f;
    public float dtCoefficient = 10f;
    public int pathVertices = 10;
    // public Vector3 pathCenter;
    public ComputePath pathDelegate = Utilities.ComputeSpiralPath;
    public bool Steer = true;
    private Vector3[] _path;
    // private Vector3 velocity;
    private Rigidbody _rigidbody;
    private Vector3 acceleration;

    void Start()
    {
        Time.timeScale = 0.75f;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
        // _rigidbody.AddForce(new Vector3(1, 1, 1), ForceMode.Impulse);
        acceleration = Vector3.zero;
        _path = new Vector3[pathVertices];
        this.pathDelegate(ref _path, Planet.center, Planet.radius * Planet.transform.localScale.x);
    }

    // pass the canvasbounds -- bounds are hard coded
    void Update()
    {
        if (Steer)
            path();
        this._rigidbody.velocity += this.acceleration;
        this._rigidbody.velocity = Vector3.ClampMagnitude(this._rigidbody.velocity, this.MaxSpeed);
        // this.transform.position += this._rigidbody.velocity;
        var p0 = this.transform.position;
        // pointer.position = p0;
        var p = p0 + this._rigidbody.velocity;
        var c = Planet.transform.position;
        var pc = p - c;
        this.transform.position = (pc.normalized * (Planet.radius * Planet.transform.localScale.x)) + this.transform.up.normalized * this.transform.localScale.y;
        acceleration *= 0;
    }

    void seek(Vector3 target)
    {
        // desired_velocity = normalize (position - target) * max_speed
        // steering = desired_velocity - velocity

        Vector3 desireVector; // this is just relative position vector
        Vector3 steeringVector;

        desireVector = (target - this.transform.position).normalized;
        desireVector *= this.MaxSpeed * Time.deltaTime; // shorthand for normalize & multiply
        steeringVector = desireVector - this._rigidbody.velocity;
        steeringVector = Vector3.ClampMagnitude(steeringVector, this.MaxAcceleration);
        this.acceleration += steeringVector;
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
        float dt = 10f;
        var p = this.transform.position + this._rigidbody.velocity * dt;
        var closest_points = closestSegment(p);
        var a = closest_points[0];
        var b = closest_points[1];
        var ab = b - a;
        var ap = p - a;
        var s = Vector3.Dot(ap, ab.normalized);
        var o = a + (s * ab.normalized);
        var e = Vector3.Distance(p, o);
        if (e >= pathRadius)
        {
            // var d = o + (dt * ab.normalized);
            var d = o + ab.normalized;
            // debug_display(a, b, d);
            seek(d);
        }
    }

    void debug_display(Vector3 a, Vector3 b, Vector3 n)
    {
        closest.position = a;
        secondClosest.position = b;
        pointer.position = n;
    }
    private List<Vector3> closestSegment(Vector3 point)
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
        Debug.Log(index.ToString());
        if (index + 1 >= _path.Length)
            return new List<Vector3> { _path[index], _path[index - 1] };
        else
            return new List<Vector3> { _path[index], _path[index + 1] };
    }

    void OnDrawGizmos()
    {
        Vector3[] path = new Vector3[pathVertices];
        this.pathDelegate(ref path, Planet.center, Planet.radius * Planet.transform.localScale.x);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(path[0], 0.2f);
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Length; i++)
            Gizmos.DrawSphere(path[i], 0.2f);
    }


    // TODO: Rewrite in c#/unity
    // void arrive(Vector3 target)
    // {
    //     Vector3 desireVector; // this is just relative position vector
    //     Vector3 steeringVector;

    //     desireVector = target - this.transform.position; // A vector pointing from the location to the newTarget
    //     float desireMagnitude = Vector3.Magnitude(desireVector);
    //     // Scale with arbitrary damping within 100 pixels
    //     if (desireMagnitude < this.radius)
    //     {
    //         var interpolatedDesireMag = map(desireMagnitude, 0, this.radius, 0, this.maxSpeed);
    //         desireVector.setMag(interpolatedDesireMag);
    //     }
    //     else
    //     {
    //         desireVector.setMag(this.maxSpeed);
    //     }
    //     steeringVector = p5.Vector.sub(desireVector, this.velocity);
    //     steeringVector.limit(this.maxAccl);
    //     this.acceleration.add(steeringVector);
    // }

    // TODO: Rewrite in c#/unity
    // flee(target)
    // {
    //     let desireVector = p5.Vector.sub(target, this.position);
    //     desireVector.normalize();
    //     desireVector.mult(-10 * this.maxSpeed);

    //     let steeringVector = p5.Vector.add(desireVector, this.velocity);
    //     steeringVector.limit();
    //     if (this.position.dist(target) <= this.fleeRadius)
    //     {
    //         this.acceleration.add(steeringVector);
    //     }
    //     else
    //     {
    //         return;
    //     }
    // }
}
