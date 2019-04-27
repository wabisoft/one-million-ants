using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Vehicle
{
    public event Action<Unit> OnUnitClicked;

    public float Damage = 1;
    // public Material selectedMat = null;
    public Shader selectedShader = null;
    public Shader notSelectedShader = null;
    public Vector3 Target;
    public bool Active { get { return gameObject.activeInHierarchy; } }

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

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (OnUnitClicked != null)
            OnUnitClicked(this);
    }
}
