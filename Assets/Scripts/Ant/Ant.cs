using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ant : Vehicle
{
    public float Damage = 1;
    protected List<Vector3> _path = null;
    public bool Active { get { return gameObject.activeInHierarchy; } }
    // public Stack<IState<Ant>> States;

    public override void Start()
    {
        base.Start();
        Rigidbody.velocity = Vector3.zero;
        Planet = Utilities.SelectPlanet(gameObject);
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.useGravity = false;
    }

    public void Attack(Base b)
    {
        b.HP -= Damage;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Spawn(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }

    public virtual void NormalizeMovement() { } // Override me for behavior!

    public virtual void Path() { }  // Override me for behavior!

}
