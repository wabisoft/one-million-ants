// Convert the 2D position of the mouse into a
// 3D position.  Display these on the game window.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    public float MaxSpeed = 7;
    public float PathRadius = 0.05f;
    public SphereCollider Sphere;
    private const int pathVertices = 100;

    private Vector3[] _path;


    private Rigidbody _rigidbody;

    private Vector3 _acceleration;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _path = new Vector3[pathVertices];
        Utilites.ComputeCirclePath(ref _path, Sphere.transform.position, Sphere.radius, pathVertices);
    }

    private void seek(Vector3 target)
    {
        Vector3 desireVector = (target - this.transform.position).normalized * MaxSpeed;
        Vector3 steeringVector = desireVector - _rigidbody.velocity;
        _rigidbody.AddForce(steeringVector);
    }


    private Vector3[] closests(Vector3 to)
    {
        float minimum = float.MaxValue;
        int index = 0;
        for (int i = 0; i < pathVertices; i++)
        {
            Debug.Log("Path " + i.ToString() + ": " + _path[i].ToString());
            Vector3 point = _path[i];
            float dist = Vector3.Distance(to, point);
            if (dist < minimum)
            {
                minimum = dist;
                index = i;
            }
        }
        Vector3[] potential = new Vector3[2];
        if (index + 1 >= pathVertices)
        {
            potential[0] = _path[index - 1];
            potential[1] = _path[0];
            // return [arrayOfPoints[index], arrayOfPoints[index - 1]];
        }
        else if (index == 0)
        {
            potential[0] = _path[pathVertices - 1];
            potential[1] = _path[0];
        }
        else
        {
            potential[0] = _path[index - 1];
            potential[1] = _path[index + 1];
        }
        minimum = float.MaxValue;
        int index2 = 0;
        for (int i = 0; i < 2; i++)
        {
            Vector3 point = potential[i];
            float dist = Vector3.Distance(to, point);
            if (dist < minimum)
            {
                minimum = dist;
                index2 = i;
            }
        }
        return new Vector3[] { _path[index], _path[index2] };
    }

    private void path()
    {
        float deltaTime = 10f;

        // kinematics with no acceleration -- x_f = x_0 + v+0(t)
        Vector3 futurePos = this.transform.position + _rigidbody.velocity * deltaTime;
        Vector3[] closests = this.closests(futurePos);
        Debug.Log("Current:" + transform.position.ToString());
        Debug.Log("Future: " + futurePos.ToString());
        Debug.Log("Closests: " + closests[0].ToString() + ", " + closests[1].ToString());
        Vector3 aa = closests[1];
        Vector3 bb = closests[0];

        Vector3 a_to_futurePos = futurePos - aa;
        Vector3 a_to_b_Segment = (bb - aa).normalized;
        a_to_b_Segment = a_to_b_Segment * Vector3.Dot(a_to_futurePos, a_to_b_Segment);

        Vector3 orthoPoint = aa + a_to_b_Segment;
        // should probably be like 2 possible velocity time steps instead of arbitrary value
        // Vector3 plusALittle = a_to_b_Segment.normalized;
        // plusALittle = plusALittle * 8;
        Vector3 newTarget = orthoPoint + a_to_b_Segment;

        Debug.Log("New Target: " + newTarget.ToString());
        float orthoHeight = Vector3.Distance(futurePos, orthoPoint);
        if (orthoHeight > PathRadius)
        {
            seek(newTarget);
        }
    }

    void OnDrawGizmos()
    {
        if (_path == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_path[0], 0.01f);
        Gizmos.color = Color.red;
        foreach (Vector3 v in _path)
        {
            Gizmos.DrawSphere(v, 0.01f);
        }
    }

    void Update()
    {

        this.path();
    }
}