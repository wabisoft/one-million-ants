
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Override the standing state for now to get transitions, but later this should probably handle it's own transtitions
public class AntPathingState : PlanetaryBodyStandingState 
{
    
    public override void Update(PlanetaryBody pb)
    {
        //base.Update(pb);
        pb.Gravitate();
        pb.StayUp();

        // somewhere deep in my loins this feel very wrong, I think it's cause I'm down casting which just seems like it should be an error
        // ^ I looked this up and it seems to be fairly exceptable, but it feels wrong and I don't like it. damned OOP.
        var ant = pb as Ant; 
        if (!ant) { 
            throw new Exception("AntPathingState.Update! Bad cast"); // Just fail your face off if this happens
        }

        ant.Path();
    }
    // TODO: ... something ... 
}

public static class AntStates
{
    public static IState<PlanetaryBody> Pathing;

    static AntStates()
    {
        Pathing = new AntPathingState();
    }

}

