using UnityEngine;
using System.Collections;

public interface IState<T>
{

    void Enter(T t); // Actions taken to enter state
    void Exit(T t);  // Actions taken to exit state

    void Update(T t);      // Let states do updating (called in MonoBehavior function of a stateful thing)
    void OnMouseDown(T t); // ^
    void OnMouseUp(T t);   // ^
    void OnMouseDrag(T t); // ^
    void OnCollisionEnter(T t, Collision c);
    /* TODO: More overrides of mono stuff */
}

