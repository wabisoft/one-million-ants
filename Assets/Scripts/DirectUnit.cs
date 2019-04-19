using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectUnit : Unit
{

    public PlanetaryBody Target;
    public Vector3 PathBack;
    public Vector3 PathFront;
    public bool PathFrontIsTarget = false;
    public int PathVertices = 10;
    //public List<Vector3> path;

    public override void Start()
    {
        base.Start();
        States.Push(UnitStates.Pathing as IState<PlanetaryBody>);
        PlanetaryBodyStates.Falling.Enter(this);
        States.Push(PlanetaryBodyStates.Falling);
        Target = GameObject.FindObjectOfType<Base>();
        SetPathPoints();
    }

    public void SetPathPoints()
    {
        var relToTarget = Target.transform.position - transform.position;
        var delta = relToTarget.magnitude / PathVertices;
        var forwardPos = transform.position + relToTarget.normalized * delta;
        var backwardPos = transform.position - relToTarget.normalized * delta;
        var adjustedRadius = Planet.Radius + transform.localScale.y;
        PathFront = (forwardPos - Planet.transform.position).normalized * adjustedRadius;
        PathBack = (backwardPos - Planet.transform.position).normalized * adjustedRadius;
      
    }

    public override void Path()
    {

        var projection = Vector3.Project(transform.position - PathBack, PathFront - PathBack);

        if ((transform.position - (PathBack + projection)).magnitude > PathRadius * 2 ||
            (PathFront - transform.position).sqrMagnitude < 0.05) {
            SetPathPoints();
        }

        Rigidbody.velocity = Steering.Path(PathBack, PathFront);
        if (Vector3.Dot(Down, Velocity) < 0.001f) { // this is true when we're just not moving. (our velocity is either straight up or straight down)
            NormalizeMovement();
            FaceFront();
            ClampSpeed();
        }
#if DEBUG
        Utilities.DebugPath(new List<Vector3> { PathBack, PathFront });
        DebugVelocity();
#endif
    }

    public override void NormalizeMovement(){
        if (Grounded()){
            //Velocity = new Vector3(Velocity.x, 0f, Velocity.z);
            // Velocity = transform.forward * Velocity.magnitude;
        }
    }

}

/* Random bull shit

 // TODO: Both Ian and Owen solution work, figure out which is faster and use it
 // Owen Solution (2 Vector normalizations)
        //        var Base = GameObject.FindObjectOfType<Base>();
        //        var relPos = Base.transform.position - transform.position;
        //        var path = new List<Vector3> {
        //             this.transform.position
        //         };
        //        if (!(relPos.sqrMagnitude <= SquaredPathRadius)) {
        //            var delta = relPos.magnitude / PathVertices;
        //            var normalizedRelPos = relPos.normalized;
        //            int verticesToCalculate = PathVertices;
        //            for (int i = 1; i < verticesToCalculate; i++) {
        //                var nextPoint = transform.position + normalizedRelPos * (delta * i);
        //                var nextPointRelPos = (nextPoint - Planet.transform.position).normalized;
        //                nextPoint = nextPointRelPos * (Planet.Radius + transform.localScale.y);
        //                Debug.DrawLine(transform.position, nextPoint, Color.black);
        //                path.Add(nextPoint);
        //            }
        //        }
        //        path.Add(base.transform.position);
        //#if DEBUG
        //        Utilities.DebugPath(path);
        //#endif

        //var steeringForce = Steering.Path(path[0], path[1]);
        //if (steeringForce.HasValue) {
        //    Velocity += steeringForce.Value;
        //} else {
        //    Velocity += transform.forward * MaxSpeed / 4.0f;
        //}
        //FaceFront();


        // Ian Solution (atan, sin and cos) // I'm pretty sure that trig methods are taylor series, so maybe same complexity as squareroot(newton's method)?
        //var Base = GameObject.FindObjectOfType<Base>();
        //var baseRelPosToPlanet = Base.transform.position - Planet.transform.position;
        //var antRelPosToPlanet = transform.position - Planet.transform.position;
        //float theta = Vector3.Angle(antRelPosToPlanet, baseRelPosToPlanet) * Mathf.Deg2Rad;
        //float dTheta = theta / (float)PathVertices;
        //float phi = Mathf.Atan((transform.position.z / transform.position.x));
        //if (transform.position.x < 0){
        //    phi += Mathf.PI;
        //}
        //var path = new List<Vector3> {
        //   this.transform.position
        //};
        //for (int i = 1; i < PathVertices; i++) {
        //   var adjustedRadius = Planet.Radius + transform.localScale.y;
        //   var x = adjustedRadius * Mathf.Sin(theta) * Mathf.Cos(phi);
        //   var y = adjustedRadius * Mathf.Cos(theta);
        //   var z = adjustedRadius * Mathf.Sin(theta) * Mathf.Sin(phi);
        //   path.Add(new Vector3(x, y, z));
        //   theta -= dTheta;
        //}
        //path.Add(Base.transform.position);
*/
