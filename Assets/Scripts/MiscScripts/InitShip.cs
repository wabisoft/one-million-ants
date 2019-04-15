using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitShip : MonoBehaviour
{
    // Start is called before the first frame update
    public SphereCollider sphere;
    private Rigidbody rb; // ship
    public Transform target; // planent

    public float speed = 7.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 angledDescentVec = transform.position - new Vector3(target.position.x, target.position.y + sphere.radius, target.position.z);
        rb.velocity = -1 * angledDescentVec.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
