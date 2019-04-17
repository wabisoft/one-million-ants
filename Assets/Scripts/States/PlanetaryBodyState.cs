using UnityEngine;
using System.Collections;


public class PlanetaryBodyStandingState : State<PlanetaryBody>
{
    public override void FixedUpdate(PlanetaryBody pb)
    {
        // Just gravitate to planet, and do nothing else;
        pb.Gravitate();
    }

    public override void OnMouseDown(PlanetaryBody pb)
    {
        pb.States.Peek().Exit(pb);
        PlanetaryBodyStates.Dragging.Enter(pb);
        pb.States.Push(PlanetaryBodyStates.Dragging as IState<PlanetaryBody>);
    }
}

public class PlanetaryBodyDraggingState : State<PlanetaryBody>
{
   /*
     * I __think__ that having these as members is going to be ok (remember that these state instances are static)
     * my rational is that the player can only drag one pb at a time 
     * and so cache these thing during the duration of this state is going to be 
     * A-OK (probably) .... guess I'll find out.
     */
    private Vector3 AxisOfRotation;
    private Vector3 PreviousPos;
    private float Theta;



    public override void OnMouseUp(PlanetaryBody pb)
    {
        //var relpos = pb.transform.position - pb.Planet.transform.position;

        //var _omega = Theta * AxisOfRotation.normalized / Time.deltaTime; // really this should be deltaTheta/deltaTime but that didn't give me the behavior I wpb. \_(?)_/
        //var tangentialVelocity = Vector3.Cross(_omega, relpos);
        //pb.Rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, pb.Planet.OrbitalVelocity);
        var moveDir = (pb.transform.position - PreviousPos) / Time.deltaTime;
        pb.Rigidbody.velocity = Vector3.ClampMagnitude(moveDir, pb.Planet.OrbitalVelocity);
        pb.States.Pop().Exit(pb);  // pop this state off the pb's state stack (say that 5 times fast)
        PlanetaryBodyStates.Falling.Enter(pb);
        pb.States.Push(PlanetaryBodyStates.Falling);
    }

    public override void OnMouseDrag(PlanetaryBody pb)
    {
        var hit = Utilities.GetPlanetHit(pb.Planet);
        if (hit.HasValue) {
            PreviousPos = pb.transform.position;
            var hitPointRelVec = hit.Value.point - pb.Planet.transform.position;
            AxisOfRotation = Vector3.Cross(pb.Up, hitPointRelVec);
            Theta = Vector3.SignedAngle(pb.Up, hitPointRelVec, AxisOfRotation);
            pb.transform.RotateAround(pb.Planet.transform.position, AxisOfRotation, Theta);
        }
    }

    public override void Enter(PlanetaryBody pb)
    {
        pb.transform.position *= 1.05f;
        // pb.transform.position += pb.Up * HeightMultiplier; // (maybe this is what I really wpb?)
        pb.Rigidbody.velocity = Vector3.zero;
    }
}

public class PlanetaryBodyFallingState : State<PlanetaryBody>
{
    public override void FixedUpdate(PlanetaryBody pb)
    {
        pb.Gravitate(Globals.Gravitation * 2f);
        pb.Drag();
        //pb.ClampSpeed(pb.Planet.OrbitalVelocity * 0.75f);
    }

    public override void OnCollisionEnter(PlanetaryBody pb, Collision collision)
    {
        Planet planet;
        if (planet = collision.gameObject.GetComponent<Planet>()) {
            pb.States.Pop().Exit(pb); // Pop falling off the ant state stack
            pb.States.Peek().Enter(pb);
        }
    }
}    


public static class PlanetaryBodyStates
{
    public static IState<PlanetaryBody> Standing;
    public static IState<PlanetaryBody> Dragging;
    public static IState<PlanetaryBody> Falling;

    static PlanetaryBodyStates()
    {
        Standing = new PlanetaryBodyStandingState();
        Dragging = new PlanetaryBodyDraggingState();
        Falling = new PlanetaryBodyFallingState();
    }
}