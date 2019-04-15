using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ant))]
public class Smashable : MonoBehaviour
{
    private Ant Ant{ get { return GetComponent<Ant>(); } }

    private void OnMouseDown()
    {
        Ant.Die();
    }
}
