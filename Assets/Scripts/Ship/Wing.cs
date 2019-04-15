using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : ShipPartBase
{
    public override void Combine(Ship ship)
    {
        ship.Attach(this);
    }
}
