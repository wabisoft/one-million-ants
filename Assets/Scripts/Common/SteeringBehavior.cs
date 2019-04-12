using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    public enum Deceleration
    {
        slow = 1,
        normal = 2,
        fast = 3
    }
    
    private Vehicle Vehicle;
    private SteeringBehavior() { } // private default ctor

    public bool On = false;

    public SteeringBehavior(Vehicle v)
    {
        Vehicle = v;
    }

    public Vector3 Seek(Vector3 target)
    {
        var desire = (Vehicle.transform.position - target).normalized * Vehicle.MaxSpeed;
        return Vehicle.Velocity - desire;
    }

    public Vector3 Arrive(Vector3 target, Deceleration deceleration = Deceleration.normal)
    {
        var target_offset = target - Vehicle.transform.position;
        if (target_offset.sqrMagnitude > 0) {
            var distance = target_offset.magnitude;
            var decelTweak = 0.3f;
            var speed = distance / decelTweak * (int)deceleration;
            var desired_velocity = (speed / distance) * target_offset;
            return desired_velocity - Vehicle.Velocity;
   
        }
        return Vector3.zero;
    }


    //protected Vector3[] PathingSegment(Vector3 point, ref IEnumerable<Vector3>path)
    //{
    //    UnityEngine.Profiling.Profiler.BeginSample("PathSegment");
    //    var min = float.MaxValue;
    //    var index = 0;
    //    for (var i = 0; i < path.Length; i++) {
    //        var dist = (point - path[i]).sqrMagnitude; //optimization (sqrt is expensive)
    //        if (dist < min) {
    //            index = i;
    //            min = dist;
    //        }
    //    }
    //    if (index + 1 >= path.Length) {
    //        return new Vector3[] { path[index], path[index] };
    //    }
    //    UnityEngine.Profiling.Profiler.EndSample();
    //    return new Vector3[] { path[index], path[index + 1] };
    //}

    public Vector3? Path(Vector3 a, Vector3 b)
    {
        // returns a steering force for following a path
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
        float dtCoefficient = 5;
        float dt = Time.deltaTime * dtCoefficient;
        // float dt = Time.deltaTime * ;
        var p = Vehicle.transform.position + Vehicle.Velocity * dt;
        var ab = b - a;
        var ap = p - a;
        // var o = a + Vector3.Project(ap, ab); // TEST TEST TEST
        var s = Vector3.Dot(ap, ab.normalized);
        var o = a + (s * ab.normalized);
        if ((p-o).sqrMagnitude >= Vehicle.SquaredPathRadius || Vector3.Angle(ab, ap) >= 15) {
            if (a == b || (a-b).sqrMagnitude < Mathf.Pow(Vehicle.ArriveRadius, 2) ){
                return Arrive(b);
            }
            else {
                var d = o + ab.normalized;
                return Seek(d);
            }
        }
        return null;
    }


}
