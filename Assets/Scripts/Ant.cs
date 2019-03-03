using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gravity))]
public class Ant : Vehicle
{
    public float Damage = 1;
    private Vector3[] _path;
    public int PathVertices = 100;
    public GameManager GameManager;

    void Start()
    {
        Rigidbody.velocity = Vector3.zero;
        Planet = Utilities.SelectPlanet(gameObject);
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.useGravity = false;
        _path = new Vector3[PathVertices];
        GeneratePath();
        // transform.position = _path[0];
    }

    public void EnterScene()
    {
        Rigidbody.MovePosition(_path[0] + transform.up.normalized);
    } 

    void FixedUpdate() {
        if (!Grounded()) {
            return;
        }

        //if (Rigidbody.velocity == Vector3.zero) {
        //    Rigidbody.velocity += Rigidbody.transform.forward * MaxSpeed / 4.0f;
        //}
        Rigidbody.velocity += Rigidbody.transform.forward * MaxSpeed / 4.0f;
        var steeringForce = Steering.Path(ref _path);
        if (steeringForce.HasValue) {
            Debug.DrawLine(Position, Position + Velocity, Color.red);
            Debug.DrawLine(Position, Position + steeringForce.Value, Color.green);
            Debug.DrawLine(Position, Position + transform.forward, Color.black);
            Velocity += steeringForce.Value;
        }
        ClampSpeed();
        FaceFront();
        DebugPath();
    }

    public void Attack(Base b)
    {
        b.HP -= Damage;
    }

    protected override void GeneratePath()
    {
        Utilities.ComputeAntiSpiralPath(ref _path, Planet.Sphere.center, Planet.Radius);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
 
    public void DebugPath()
    {
        for (int i = 1; i < _path.Length; i++)
        {
            Debug.DrawLine(_path[i - 1], _path[i], Color.red);
        }
    } 
}
