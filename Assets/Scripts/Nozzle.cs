﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozzle : MonoBehaviour, IShipPart
{
    public void Combine(Ship ship)
    {
        ship.attach(this);
    }
}