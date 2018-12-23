using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilites
{

    // Doesn't get a real geodesic
    // just computes a circle on the centering from center
    public static void ComputeCirclePath(ref Vector3[] arr, Vector3 center, float radius, int size)
    {
        float theta = 0;
        float step = 2 * Mathf.PI / size;
        for (int i = 0; theta < 2 * Mathf.PI; i++)
        {
            arr[i] = new Vector3(center.x + Mathf.Cos(theta) * radius, center.y + Mathf.Sin(theta) * radius, center.z);
            theta += step;
        }
    }
}