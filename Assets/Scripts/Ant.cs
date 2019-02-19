using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gravity))]
public class Ant : Vehicle
{
    public float Damage = 10;

    public void Attack(Base b)
    {
        b.HP -= Damage;
    }

    void OnCollisionEnter(Collision collision)
    {
        var p = collision.gameObject.GetComponent<Planet>();  
        if (p) {
            StartSteering();
        }
        var b = collision.gameObject.GetComponent<Base>();  
        if (b) {
            Debug.Log("asdfjkl");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var p = collision.gameObject.GetComponent<Planet>();  
        if (p) {
            StopSteering();
        }
    }

    protected override void GeneratePath()
    {
        Utilities.ComputeAntiSpiralPath(ref _path, _planet.Sphere.center, _planet.Radius);
    }
}
