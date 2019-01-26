using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComputePath(ref Vector3[] arr, Vector3 center, float radius);

public class Utilities
{

    public static float Epsilon = 1.0001f;

    // Doesn't get a real geodesic
    // just computes a circle on the centering from center
    public static void ComputeCirclePath(ref Vector3[] arr, Vector3 center, float radius)
    {
        radius *= Epsilon;
        float theta = 0;
        float step = 2 * Mathf.PI / arr.Length;
        // for (int i = 0; theta < 2 * Mathf.PI; i++)
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new Vector3(center.x + Mathf.Cos(theta) * radius, center.y + Mathf.Sin(theta) * radius, center.z);
            theta += step;
        }
    }

    public static void ComputeSpiralPath(ref Vector3[] arr, Vector3 center, float radius)
    {
        radius *= Epsilon;
        float numberOfTurns = 5.0f;
        float parameter = -radius;
        float dparameter = -7.0f * (parameter / arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new Vector3(
                Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Cos(numberOfTurns * Mathf.PI * parameter / radius),
                parameter,
                Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Sin(numberOfTurns * Mathf.PI * parameter / radius)
            );
            parameter += dparameter;
        }
    }

}