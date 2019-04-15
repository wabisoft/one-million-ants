using UnityEngine;
using System.Collections;

public abstract class PlanetaryBodyState : IState<PlanetaryBody>
{
    /* 
       Shitty default interface :\ 
       Means we only have to impliment the parts of IState we care about
       for each state.
    */
    public virtual void Enter(PlanetaryBody t) { }
    public virtual void Exit(PlanetaryBody t) { }
    public virtual void OnCollisionEnter(PlanetaryBody t, Collision c) { }
    public virtual void OnMouseDown(PlanetaryBody t) { }
    public virtual void OnMouseDrag(PlanetaryBody t) { }
    public virtual void OnMouseUp(PlanetaryBody t) { }
    public virtual void Update(PlanetaryBody t) { }
}

public class PlanetaryBodyStandingState : PlanetaryBodyState
{
    public override void Update(PlanetaryBody pb)
    {
        // Just gravitate to planet, and do nothing else;
        pb.Gravitate();
    }

    public override void OnMouseDown(PlanetaryBody pb)
    {
        PlanetaryBodyStates.Dragging.Enter(pb);
        pb.States.Push(PlanetaryBodyStates.Dragging as IState<PlanetaryBody>);
    }
}

public class PlanetaryBodyDraggingState : PlanetaryBodyState
{
   /*
     * I __think__ that having these as members is going to be ok (remember that these state instances are static)
     * my rational is that the player can only drag one pb at a time 
     * and so cache these thing during the duration of this state is going to be 
     * A-OK (probably) .... guess I'll find out.
     */
    private Vector3 AxisOfRotation;
    private float Theta;



    public override void OnMouseUp(PlanetaryBody pb)
    {
        var relpos = pb.transform.position - pb.Planet.transform.position;

        var _omega = Theta * AxisOfRotation.normalized; // really this should be deltaTheta/deltaTime but that didn't give me the behavior I wpb. \_(?)_/
        var tangentialVelocity = Vector3.Cross(_omega, relpos);
        pb.Rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, pb.Planet.OrbitalVelocity);

        pb.States.Pop();  // pop this state of the pb's state stack (say that 5 times fast)
        PlanetaryBodyStates.Falling.Enter(pb);
        pb.States.Push(PlanetaryBodyStates.Falling);
    }

    public override void OnMouseDrag(PlanetaryBody pb)
    {
        var hit = Utilities.GetPlanetHit(pb.Planet);
        if (hit.HasValue) {
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

public class PlanetaryBodyFallingState : PlanetaryBodyState
{
    public override void Update(PlanetaryBody pb)
    {
        pb.Gravitate();
        pb.ClampSpeed();
    }

    public override void OnCollisionEnter(PlanetaryBody pb, Collision collision)
    {
        Planet planet;
        if (planet = collision.gameObject.GetComponent<Planet>()) {
            pb.States.Pop(); // Pop falling off the ant state stack
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