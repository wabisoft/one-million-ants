using UnityEngine;
using System;


public interface IMonoBehaviourMotion<T>
{
    void Enter(T t);
    void Exit(T t);
    void FixedUpdate(T t);
    void OnTriggerEnter(T t);
    void OnCollisionEnter(T t, Collision c);
    void OnCollisionExit(T t, Collision c);
    void OnCollisionStay(T t, Collision c);
    void OnMouseDown(T t);
    void OnMouseDrag(T t);
    void OnMouseUp(T t);
    void Update(T t);
}

# region MonoBehaviour


public abstract class MonoBehaviourMotion : IMonoBehaviourMotion<MonoBehaviour>
{
    public virtual void Enter(MonoBehaviour mb) { }
    public virtual void Exit(MonoBehaviour mb) { }
    public virtual void FixedUpdate(MonoBehaviour mb) { }
    public virtual void OnCollisionEnter(MonoBehaviour mb, Collision c) { }
    public virtual void OnCollisionExit(MonoBehaviour mb, Collision c) { }
    public virtual void OnCollisionStay(MonoBehaviour mb, Collision c) { }
    public virtual void OnMouseDown(MonoBehaviour mb) { }
    public virtual void OnMouseDrag(MonoBehaviour mb) { }
    public virtual void OnMouseUp(MonoBehaviour mb) { }
    public virtual void OnPlanetClicked(MonoBehaviour mb) { }

    public void OnTriggerEnter(MonoBehaviour t){ }

    public virtual void Update(MonoBehaviour mb) { }
}

# endregion

#region PlanetaryBody
public abstract class PlanetaryBodyMotion : MonoBehaviourMotion, IMonoBehaviourMotion<PlanetaryBody>
{
    public virtual void Enter(PlanetaryBody pb) { }
    public virtual void Exit(PlanetaryBody pb) { }
    public virtual void FixedUpdate(PlanetaryBody pb) { }
    public virtual void OnCollisionEnter(PlanetaryBody pb, Collision c) { }
    public virtual void OnCollisionExit(PlanetaryBody pb, Collision c) { }
    public virtual void OnCollisionStay(PlanetaryBody pb, Collision c) { }
    public virtual void OnMouseDown(PlanetaryBody pb) { }
    public virtual void OnMouseDrag(PlanetaryBody pb) { }
    public virtual void OnMouseUp(PlanetaryBody pb) { }
    public virtual void OnPlanetClicked(PlanetaryBody pb) { }
    public virtual void Update(PlanetaryBody pb) { }
    public virtual void OnTriggerEnter(PlanetaryBody pb) { }
}

public class PlanetaryBodyStanding : PlanetaryBodyMotion
{
    public override void FixedUpdate(PlanetaryBody pb)
    {
        // Just gravitate to planet, and do nothing else;
        pb.Gravitate();
    }

    public override void OnMouseDown(PlanetaryBody pb)
    {
        pb.Motions.Peek().Exit(pb);
        PlanetaryBodyMotions.Dragging.Enter(pb);
        pb.Motions.Push(PlanetaryBodyMotions.Dragging);
    }
}

public class PlanetaryBodyDragging : PlanetaryBodyMotion
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
        pb.Motions.Pop().Exit(pb);  // pop this state off the pb's state stack (say that 5 times fast)
        PlanetaryBodyMotions.Falling.Enter(pb);
        pb.Motions.Push(PlanetaryBodyMotions.Falling);
    }

    public override void OnMouseDrag(PlanetaryBody pb)
    {
        var hit = Utilities.GetAnyPlanetHit(pb.Planet);
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

public class PlanetaryBodyFalling : PlanetaryBodyMotion
{
    public override void FixedUpdate(PlanetaryBody pb)
    {
        pb.Gravitate(Globals.Gravitation);
        // pb.Drag();
        //pb.ClampSpeed(pb.Planet.OrbitalVelocity * 0.75f);
    }

    public override void OnCollisionEnter(PlanetaryBody pb, Collision collision)
    {
        Planet planet;
        if (planet = collision.gameObject.GetComponent<Planet>()) {
            pb.Motions.Pop().Exit(pb); // Pop falling off the ant state stack
            pb.Motions.Peek().Enter(pb);
        }
    }
}


public static class PlanetaryBodyMotions
{
    public static PlanetaryBodyMotion Standing;
    public static PlanetaryBodyMotion Dragging;
    public static PlanetaryBodyMotion Falling;

    static PlanetaryBodyMotions()
    {
        Standing = new PlanetaryBodyStanding();
        Dragging = new PlanetaryBodyDragging();
        Falling = new PlanetaryBodyFalling();
    }
}

# endregion

# region Unit


public abstract class UnitMotion : PlanetaryBodyMotion, IMonoBehaviourMotion<Unit>
{
    public virtual void Enter(Unit unit) { }
    public virtual void Exit(Unit unit) { }
    public virtual void FixedUpdate(Unit unit) { }
    public virtual void OnCollisionEnter(Unit unit, Collision c) { }
    public virtual void OnCollisionExit(Unit unit, Collision c) { }
    public virtual void OnCollisionStay(Unit unit, Collision c) { }
    public virtual void OnMouseDown(Unit unit) { }
    public virtual void OnMouseDrag(Unit unit) { }
    public virtual void OnMouseUp(Unit unit) { }
    public virtual void OnPlanetClicked(Unit unit) { }
    public void OnTriggerEnter(Unit unit) { }
    public virtual void Update(Unit unit) { }
}


// Override the standing state for now to get transitions, but later this should probably handle it's own transtitions
public class UnitStanding : UnitMotion 
{
    public override void FixedUpdate(Unit unit)
    {
        PlanetaryBodyMotions.Standing.FixedUpdate(unit);
    }

    public override void OnMouseDown(Unit pb)
    {
        // TODO: select I think?
        // We need a way to get the next click, regardless of where it is.
        // hmmm, maybe this whole hooking in unity event system is the wrong move afterall.
        pb.Motions.Pop().Exit(pb);
        UnitMotions.Selected.Enter(pb);
        pb.Motions.Push(UnitMotions.Selected);
    }
}

public class UnitPathing : UnitStanding 
{
    
    public override void FixedUpdate(Unit unit)
    {
        base.FixedUpdate(unit);
        unit.StayUp();
        unit.Path();
    }

    // TODO: ... something ... 
}

public static class UnitMotions
{
    public static UnitMotion Standing;
    public static UnitMotion Selected;
    public static UnitMotion Pathing;

    static UnitMotions()
    {
        Standing = new UnitStanding();
        Pathing = new UnitPathing();
    }

}

# endregion

# region Base 

// Override the standing state for now to get transitions, but later this should probably handle it's own transtitions

public abstract class BaseMotion : PlanetaryBodyMotion, IMonoBehaviourMotion<Base>
{
    public virtual void Enter(Base b) { }
    public virtual void Exit(Base b) { }
    public virtual void FixedUpdate(Base b) { }
    public virtual void OnCollisionEnter(Base b, Collision c) { }
    public virtual void OnCollisionExit(Base b, Collision c) { }
    public virtual void OnCollisionStay(Base b, Collision c) { }
    public virtual void OnMouseDown(Base b) { }
    public virtual void OnMouseDrag(Base b) { }
    public virtual void OnMouseUp(Base b) { }
    public virtual void OnPlanetClicked(Base b) { }
    public void OnTriggerEnter(Base b) { }
    public virtual void Update(Base b) { }
}


public class BaseStanding : BaseMotion
{
    // TODO: ... something ... 
    // A lot of the player interaction with base is gonna be coded here
    public override void FixedUpdate(Base b)
    {
        PlanetaryBodyMotions.Standing.FixedUpdate(b);
    }
}

public static class BaseMotions
{
    public static PlanetaryBodyMotion Standing;

    static BaseMotions()
    {
        Standing = new BaseStanding();
    }

}


# endregion


//public class UnitSelectedState : UnitMotion
//{
//    public override void Update(PlanetaryBody pb)
//    {
//        var directUnit = pb as DirectUnit;
//        if (!directUnit) {
//            throw new Exception("UnitPathingState.Update! Bad cast"); // Just fail your face off if this happens
//        }
//        // Poll for a mouse click and path to that position;
//    //    if (Input.GetMouseButtonDown(0)) {
//    //        var hit = Utilities.GetTopPlanetHit(directUnit.Planet);
//    //        if (hit.HasValue) {
//    //            Debug.DrawLine(pb.transform.position, hit.Value.point, Color.magenta);
//    //            directUnit.Target = hit.Value.point;
//    //        }
//    //        directUnit.States.Pop().Exit(directUnit);
//    //        UnitStates.Pathing.Enter(directUnit);
//    //        pb.States.Push(UnitStates.Pathing);
//    //    }
//    }

//    public override void Enter(PlanetaryBody pb)
//    {
//        // TODO: Set a shader maybe to show that this unit is selected
//        var unit = pb as Unit;
//        if (!unit) {
//            throw new Exception("UnitPathingState.Update! Bad cast"); // Just fail your face off if this happens
//        }
//        //directUnit.GetComponent<Renderer>().material = directUnit.selectedMat;
//        unit.GetComponent<Renderer>().material.shader = unit.selectedShader;
//    }

//    public override void Exit(PlanetaryBody pb)
//    {
//        var unit = pb as Unit;
//        if (!unit) {
//            throw new Exception("UnitPathingState.Update! Bad cast"); // Just fail your face off if this happens
//        }
//        unit.GetComponent<Renderer>().material.shader = unit.notSelectedShader; 
//    }

//    public override void OnPlanetClicked(PlanetaryBody pb)
//    {
//        var unit = pb as Unit;
//        if (!unit) {
//            throw new Exception("UnitPathingState.Update! Bad cast"); // Just fail your face off if this happens
//        }
//        var hit = Utilities.GetTopPlanetHit(unit.Planet);
//        if (hit.HasValue) {
//            unit.Target = hit.Value.point;
//            unit.States.Pop().Exit(unit);
//            UnitStates.Pathing.Enter(unit);
//            unit.States.Push(UnitStates.Pathing);
//        }
        
//        Debug.Log("Planet was clicked after selecting a unit");
//    }

//    public override void OnMouseDown(PlanetaryBody pb)
//    {
//        // TODO: Deselect maybe?
//        pb.States.Pop().Exit(pb);
//        UnitStates.Standing.Enter(pb);
//        pb.States.Push(UnitStates.Standing);

//    }
//}


