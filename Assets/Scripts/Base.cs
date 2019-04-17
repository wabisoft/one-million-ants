using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : PlanetaryBody 
{
    public float HP = 100;

    public override void Start()
    {
        base.Start();
        States.Pop();
        States.Push(BaseStates.Standing);
    }
}
