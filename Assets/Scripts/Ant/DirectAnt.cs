using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAnt: Ant
{
    protected override void GeneratePath()
    {
        if (_path == null) {
            _path = new Vector3[PathVertices];
            Utilities.ComputeAntiSpiralPath(ref _path, Planet.Sphere.center, Planet.Radius);
        }
    }
}
