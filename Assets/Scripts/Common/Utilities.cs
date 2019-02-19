using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ComputePath(ref Vector3[] arr, Vector3 center, float radius);

public class Utilities
{

    public static float Epsilon = 1.01f;

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

    public static void ComputeAntiSpiralPath(ref Vector3[] arr, Vector3 center, float radius)
    {
        radius *= Epsilon;
        float numberOfTurns = 5.0f;
        float parameter = -radius;
        float dparameter = 2 * radius / arr.Length;
        for (int i = 0; i < arr.Length; i++)
        {
            var x = Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Cos(numberOfTurns * Mathf.PI * parameter / radius);
            var z = Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Sin(numberOfTurns * Mathf.PI * parameter / radius);
            arr[i] = new Vector3(
                z,
                parameter,
                x
            );
            parameter += dparameter;
        }
    }

    public static void ComputeSpiralPath(ref Vector3[] arr, Vector3 center, float radius)
    {
        radius *= Epsilon;
        float numberOfTurns = 5.0f;
        float parameter = -radius;
        float dparameter = 2 * radius / arr.Length;
        for (int i = 0; i < arr.Length; i++)
        {
            var x = Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Cos(numberOfTurns * Mathf.PI * parameter / radius);
            var z = Mathf.Sqrt(Mathf.Pow(radius, 2.0f) - Mathf.Pow(parameter, 2.0f)) * Mathf.Sin(numberOfTurns * Mathf.PI * parameter / radius);
            arr[i] = new Vector3(
                x,
                parameter,
                z
            );
            parameter += dparameter;
        }
        Debug.Log("bleh");
    }

    public static void ComputeStraightPath(ref Vector3[] arr, Vector3 start, float pathLength)
    {
        arr[0] = start;
        var step = pathLength / arr.Length;
        for (int i = 1; i < arr.Length; i++)
        {
            arr[i] = new Vector3(arr[i - 1].x + step, arr[i - 1].y, arr[i - 1].z + step);
        }
    }


    public static Planet SelectPlanet(GameObject target)
    {
        Planet p = null;
        var minDist = float.PositiveInfinity;
        foreach(var planet in GameObject.FindObjectsOfType<Planet>())
        {
            if ((target.transform.position - planet.transform.position).sqrMagnitude < minDist) { p = planet; }
        }
        return p;
    }
}