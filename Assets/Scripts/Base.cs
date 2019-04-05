using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float HP = 100;

    private void Update()
    {
        if (HP <= 0) {
            //Debug.Log("GAME OVER");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var a = collision.gameObject.GetComponent<Ant>();
        if (a) {
            a.Attack(this);
            // GameObject.Destroy(a.gameObject);
        }
        
    }

    public List<Vector3> PathToMeFromPoint(Vector3 point)
    {
        var ret = new List<Vector3>();
        
        return ret;
    }
}
