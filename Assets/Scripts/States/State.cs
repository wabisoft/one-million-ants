using UnityEngine;
using System.Collections;

public interface IState<T>
{

    void Enter(T t); // Actions taken to enter state
    void Exit(T t);  // Actions taken to exit state

    void FixedUpdate(T t);
    void OnMouseDown(T t); // ^
    void OnMouseUp(T t);   // ^
    void OnMouseDrag(T t); // ^
    void OnCollisionEnter(T t, Collision c);
    void OnCollisionExit(T t, Collision c);
    void OnCollisionStay(T t, Collision c);
    void Update(T t);      // Let states do updating (called in MonoBehavior function of a stateful thing)
    /* TODO: More overrides of mono stuff */
}

public abstract class State<T> : IState<T>
{
    public virtual void Enter(T t) { }
    public virtual void Exit(T t) { }
    public virtual void FixedUpdate(T t) { }
    public virtual void OnCollisionEnter(T t, Collision c) { }
    public virtual void OnCollisionExit(T t, Collision c) { }
    public virtual void OnCollisionStay(T t, Collision c) { }
    public virtual void OnMouseDown(T t) { }
    public virtual void OnMouseDrag(T t) { }
    public virtual void OnMouseUp(T t) { }
    public virtual void Update(T t) { }
}


