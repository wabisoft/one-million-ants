
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AntState : IState<Ant>
{
    /* 
       Shitty default interface :\ 
       Means we only have to impliment the parts of IState we care about
       for each state.
    */
    public virtual void Enter(Ant ant) { }

    public virtual void Exit(Ant ant) { }

    public virtual void OnCollisionEnter(Ant ant, Collision c) { }

    public virtual void OnMouseDown(Ant ant) { }

    public virtual void OnMouseDrag(Ant ant) { }

    public virtual void OnMouseUp(Ant ant) { }

    public virtual void Update(Ant ant) { }

}

public class AntStandingState : AntState
{
    public override void Update(Ant ant)
    {
        // Just gravitate to planet, and do nothing else;
        ant.Gravitate();
    }

    public override void OnMouseDown(Ant ant)
    {
        AntStates.Dragging.Enter(ant);
        ant.States.Push(AntStates.Dragging);
    }
}


public class AntDraggingState : AntState
{
    /*
     * I __think__ that having these as members is going to be ok (remember that these state instances are static)
     * my rational is that the player can only drag one ant at a time 
     * and so cache these thing during the duration of this state is going to be 
     * A-OK (probably) .... guess I'll find out.
     */
    private Vector3 AxisOfRotation;
    private float Theta;



    public override void OnMouseUp(Ant ant)
    {
        var relpos = ant.transform.position - ant.Planet.transform.position;

        var _omega = Theta * AxisOfRotation.normalized; // really this should be deltaTheta/deltaTime but that didn't give me the behavior I want. \_(?)_/
        var tangentialVelocity = Vector3.Cross(_omega, relpos);
        ant.Rigidbody.velocity = Vector3.ClampMagnitude(tangentialVelocity, ant.Planet.OrbitalVelocity);

        ant.States.Pop();  // pop this state of the ant's state stack (say that 5 times fast)
        ant.States.Push(AntStates.Falling);
        // I though about checking if the stack is empty and pushing a default state here
        // But I think I want any errors to fail really hard for now. Maybe later I'll make this safer ;)
    }

    public override void OnMouseDrag(Ant ant)
    {
        var hit = Utilities.GetPlanetHit(ant.Planet);
        if (hit.HasValue) {
            var hitPointRelVec = hit.Value.point - ant.Planet.transform.position;
            AxisOfRotation = Vector3.Cross(ant.Up, hitPointRelVec);
            Theta = Vector3.SignedAngle(ant.Up, hitPointRelVec, AxisOfRotation);
            ant.transform.RotateAround(ant.Planet.transform.position, AxisOfRotation, Theta);
        }
    }

    public override void Enter(Ant ant)
    {
        ant.transform.position *= 1.05f;
        // ant.transform.position += ant.Up * HeightMultiplier; // (maybe this is what I really want?)
        ant.Rigidbody.velocity = Vector3.zero;
    }

}




public class AntFallingState : AntState
{
    public override void Update(Ant ant)
    {
        ant.Gravitate();
        ant.ClampSpeed();
    }

    public override void OnCollisionEnter(Ant ant, Collision collision)
    {
        Planet planet;
        if(planet = collision.gameObject.GetComponent<Planet>()){
            ant.States.Pop(); // Pop falling off the ant state stack
        } 
    }

}

public class AntPathingState : AntState
{
    // TODO: ... something ... 
}

public static class AntStates
{
    public static AntStandingState Standing;
    public static AntDraggingState Dragging;
    public static AntPathingState Pathing;
    public static AntFallingState Falling;

    static AntStates()
    {
        Standing = new AntStandingState();
        Dragging = new AntDraggingState();
        Pathing = new AntPathingState();
        Falling = new AntFallingState();
    }

}

