using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAnt: Ant
{

    public PlanetaryBody Target;
    public Vector3 PathBack;
    public Vector3 PathFront;
    public bool PathFrontIsTarget = false;
    public int PathVertices = 10;

    public override void Start()
    {
        base.Start();
        States.Push(AntStates.Pathing as IState<PlanetaryBody>);
        Target = GameObject.FindObjectOfType<Base>();
        SetPathPoints();
    }

    public void SetPathPoints()
    {
        var baseRelPosToPlanet = Target.transform.position - Planet.transform.position;
        var antRelPosToPlanet = transform.position - Planet.transform.position;
        float theta = Vector3.Angle(antRelPosToPlanet, baseRelPosToPlanet) * Mathf.Deg2Rad;
        float dTheta = theta / (float)PathVertices;
        float phi = Mathf.Atan((transform.position.z / transform.position.x));
        if (transform.position.x < 0){
            phi += Mathf.PI;
        }
        var adjustedRadius = Planet.Radius + transform.localScale.y;

        if (dTheta * Mathf.Rad2Deg < 5) {
            PathBack = Utilities.SphericalToCartesian(adjustedRadius, theta + 5*Mathf.Rad2Deg, phi);
            PathFront = Target.transform.position;
            PathFrontIsTarget = true;
        } else {
            PathBack = Utilities.SphericalToCartesian(adjustedRadius, theta + dTheta, phi);
            PathFront = Utilities.SphericalToCartesian(adjustedRadius, theta - dTheta, phi);
            PathFrontIsTarget = false;
        }
    }

    public override void Path()
    {

        if (! PathFrontIsTarget){
            var projection = Vector3.Project(transform.position - PathBack, PathFront - PathBack);
            Debug.DrawLine(transform.position, projection, Color.magenta);
            Debug.DrawLine(transform.position, PathBack + projection, Color.green);
            if ((transform.position - (PathBack + projection)).magnitude > PathRadius + 0.1 ||
                (PathFront - transform.position).sqrMagnitude < 0.05){
                SetPathPoints();
            }
        }

        Velocity = Steering.Path(PathBack, PathFront);
        if (Vector3.Cross(Down, Velocity) != Vector3.zero) {
            NormalizeMovement();
            FaceFront();
        }
        ClampSpeed();
#if DEBUG
        Utilities.DebugPath(new List<Vector3> { PathBack, PathFront });
#endif
    }

    public override void NormalizeMovement(){
        if (Grounded()){
            Velocity = transform.forward * Velocity.magnitude;
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
