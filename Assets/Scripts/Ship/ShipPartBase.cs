using UnityEngine;
using System.Collections;

public abstract class ShipPartBase : PlanetaryBody, IShipPart
{

    private void Start()
    {
        var spawn = Planet.Random();
        var relPos = spawn - Planet.transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, relPos.normalized) * transform.rotation;
        // Slerp to new rotation
        transform.rotation = targetRotation;
        spawn += transform.up.normalized * transform.localScale.y;
        transform.position = spawn;
    }
    

    public abstract void Combine(Ship ship);
}
