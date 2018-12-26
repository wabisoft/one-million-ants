
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SteeringController : MonoBehaviour
{
    public float MaxSpeed = 1.4f; // Walking speed
    public float MaxAcceleration = 0.25f; // These seems to be a good cap for the above speed
    public SphereCollider Planet;
    // public float radius = 4f;
    public float fleeRadius = 4f;
    public float pathRadius = .05f;
    public int pathVertices = 10;
    // public Vector3 pathCenter;
    public ComputePath pathDelegate;
    private Vector3[] _path;
    // private Vector3 velocity;
    private Rigidbody _rigidbody;
    private Vector3 acceleration;

    void Start()
    {
        // velocity = new Vector3(1, 1, 1) * 2;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.useGravity = false;
        // _rigidbody.AddForce(new Vector3(1, 1, 1), ForceMode.Impulse);
        acceleration = Vector3.zero;
        _path = new Vector3[pathVertices];
        Utilities.ComputeSpiralPath(ref _path, Planet.center, Planet.radius * Planet.transform.localScale.x);
    }

    // pass the canvasbounds -- bounds are hard coded
    void Update()
    {
        path();
        // TODO: Figure out how to use forces when seeking maybe? so we don't have to do all this
        // this._rigidbody.velocity += this.acceleration;
        // this._rigidbody.velocity = Vector3.ClampMagnitude(this._rigidbody.velocity, this.MaxSpeed);
        // this.transform.position += this._rigidbody.velocity;
        this.acceleration *= 0;
    }

    void seek(Vector3 target)
    {
        Vector3 desireVector; // this is just relative position vector
        Vector3 steeringVector;

        desireVector = (target - this.transform.position).normalized;
        desireVector *= this.MaxSpeed; // shorthand for normalize & multiply
        steeringVector = desireVector - this._rigidbody.velocity;
        steeringVector = Vector3.ClampMagnitude(steeringVector, this.MaxAcceleration);
        this.acceleration += steeringVector;
        this._rigidbody.AddForce(this.acceleration, ForceMode.Acceleration);
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


    void path()
    {
        float deltaTime = 10;

        Vector3 velocityCopy = this._rigidbody.velocity;
        // kinematics with no acceleration -- x_f = x_0 + v+0(t)
        velocityCopy *= deltaTime;
        Vector3 futurePos = this.transform.position + velocityCopy;

        Vector3 aa;
        Vector3 bb;
        Utilities.ClosestSegment(subject: futurePos, candidates: _path, closest: out aa, secondClosest: out bb);

        Vector3 a_to_futurePos = futurePos - aa;
        Vector3 a_to_b_Segment = (bb - aa).normalized;
        a_to_b_Segment *= Vector3.Dot(a_to_futurePos, a_to_b_Segment);

        Vector3 orthoPoint = aa + a_to_b_Segment;
        // should probably be like 2 possible velocity time steps instead of arbitrary value
        Vector3 plusALittle = a_to_b_Segment.normalized;
        plusALittle *= 8;
        Vector3 newTarget = orthoPoint + a_to_b_Segment;

        float orthoHeight = Vector3.Distance(futurePos, orthoPoint);
        if (orthoHeight > this.pathRadius)
        {
            this.seek(newTarget);
        }
    }

    void OnDrawGizmos()
    {
        Vector3[] path = new Vector3[pathVertices];
        Utilities.ComputeSpiralPath(ref path, Planet.center, Planet.radius * Planet.transform.localScale.x);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(path[0], 0.2f);
        Gizmos.color = Color.red;
        for (int i = 1; i < path.Length; i++)
            Gizmos.DrawSphere(path[i], 0.2f);
    }

    void MakeSphere(Vector3 position, float arg_radius)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.GetComponent<SphereCollider>().radius = arg_radius;
    }
}
