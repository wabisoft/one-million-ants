using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAnt: Ant
{
    public int PathVertices = 10;
    protected override void Path()
    {

        // TODO: Both Ian and Owen solution work, figure out which is faster and use it

        // Owen Solution
        var Base = GameObject.FindObjectOfType<Base>();
        var relPos = Base.transform.position - transform.position;
        var path = new List<Vector3> {
             this.transform.position
         };
        if (!(relPos.sqrMagnitude <= SquaredPathRadius)) {
            var delta = relPos.magnitude / PathVertices;
            var normalizedRelPos = relPos.normalized;
            int verticesToCalculate = PathVertices;
            for (int i = 1; i < verticesToCalculate; i++) {
                var nextPoint = transform.position + normalizedRelPos * (delta * i);
                var nextPointRelPos = (nextPoint - Planet.transform.position).normalized;
                nextPoint = nextPointRelPos * (Planet.Radius + transform.localScale.y);
                Debug.DrawLine(transform.position, nextPoint, Color.black);
                path.Add(nextPoint);
            }
        }
        path.Add(base.transform.position);
        DebugPath(path);

        //var steeringForce = Steering.Path(path[0], path[1]);
        //if (steeringForce.HasValue) {
        //    Velocity += steeringForce.Value;
        //} else {
        //    Velocity += transform.forward * MaxSpeed / 4.0f;
        //}
        //FaceFront();


        // Ian Solution (Something wrong with theta calculation)
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

        // var steeringForce = Steering.Path(path[0], path[1]);
        // if (steeringForce.HasValue){
        //     Velocity += steeringForce.Value;
        // } else {
        //     Velocity += transform.forward * MaxSpeed / 4.0f;
        // }
        FaceFront();
#if DEBUG
        DebugPath(path);
#endif
    }

    protected override void NormalizeMovement(){
        if (Grounded()){
            Velocity = transform.forward * Velocity.magnitude;
        }
    }

}
