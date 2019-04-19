
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Override the standing state for now to get transitions, but later this should probably handle it's own transtitions
public class BaseStandingState : PlanetaryBodyStandingState 
{
    // TODO: ... something ... 
    // A lot of the player interaction with base is gonna be coded here
    public override void Update(PlanetaryBody pb)
    { }

    public override void OnMouseDown(PlanetaryBody pb)
    { }
}

public static class BaseStates
{
    public static IState<PlanetaryBody> Standing;

    static BaseStates()
    {
       Standing = new BaseStandingState();
    }

}

