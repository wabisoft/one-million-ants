using UnityEngine;
using System.Collections;

public class SphericalPath : MonoBehaviour
{

    SphereCollider myCollider;
    private float _radius;
    public Vector3 testPoint;
    public float deltaTheta = Mathf.PI/10.0f; // 10 points
    Vector3[] points;

    void Start()
    {
        myCollider = transform.GetComponent<SphereCollider>();
        _radius = myCollider.radius;
        testPoint = new Vector3(0.0f, 0.0f, _radius);
        points = new Vector3[10];
        makePoints(testPoint);
    }

    void makePoints(Vector3 somePoint)
    {
        // non signed angle between y-axis and point
        float theta = Vector3.Angle(somePoint, Vector3.up);
        theta *= Mathf.Deg2Rad;
        // const angle on x-z plane
        float phi = Mathf.Atan(somePoint.x / somePoint.z);
        phi *= Mathf.Deg2Rad;

        //Debug.Log("Theta equals: " + theta);
        //Debug.Log("Phi equals: " + phi);
        //Debug.Log("Radius equals: " + _radius);

        // if you want to always end at pole: theta --> 0
        // while(theta > 0.0f)
        //{
        //}
        for (int i = 0; i < 10; i++)
        {
            float xx = _radius * Mathf.Sin(theta) * Mathf.Cos(phi);
            float zz = _radius * Mathf.Sin(theta) * Mathf.Sin(phi);
            float yy = _radius * Mathf.Cos(theta);

            Vector3 point = new Vector3(xx, yy, zz);
            points[i] = point;
            //Debug.Log(point.ToString());
            theta -= deltaTheta;
        }
     }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (points != null)
        {
            for (int i = 0; i < points.Length; i++)
            {
                //Debug.Log("hmm");
                Gizmos.DrawSphere(points[i], 0.1f);
            }
        }
    }
}
