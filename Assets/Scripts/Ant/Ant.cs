using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ant : Vehicle
{
    public float Damage = 1;
    protected List<Vector3> _path = null;
    public bool Active { get { return gameObject.activeInHierarchy; } }
    public Stack<IState<Ant>> States;

    void Start()
    {
        Rigidbody.velocity = Vector3.zero;
        Planet = Utilities.SelectPlanet(gameObject);
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.useGravity = false;
        States = new Stack<IState<Ant>>();
        States.Push(AntStates.Standing);
        // transform.position = _path[0];
    }

    private void Update()
    {
        States.Peek().Update(this);
        Debug.Log(States.Peek().ToString());
    }

    private void OnMouseDown()
    { 
        States.Peek().OnMouseDown(this);
    }

    private void OnMouseDrag()
    {
        States.Peek().OnMouseDrag(this);
    }

    private void OnMouseUp()
    {
        States.Peek().OnMouseUp(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        States.Peek().OnCollisionEnter(this, collision);
    }

    // public void EnterScene()
    // {
    //     Rigidbody.MovePosition(_path[0] + transform.up.normalized);
    // } 

    // void Update() {
    //    if (!Grounded()) {
    //        Gravitate();
    //        return;
    //    }
    //   // StayUp();
    //   // StayGrounded();
    //    Path();
    //    ClampSpeed();
    //    NormalizeMovement();
    // }

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

    public void DebugPath(List<Vector3> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(path[i - 1], path[i], Color.red);
        }
    } 

    protected abstract void Path();

    protected abstract void NormalizeMovement();

}
