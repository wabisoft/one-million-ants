using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{

    // Doesn't get a real geodesic
    // just computes a circle on the centering from center
    public static void ComputeCirclePath(ref Vector3[] arr, Vector3 center, float radius)
    {
        float theta = 0;
        float step = 2 * Mathf.PI / arr.Length;
        for (int i = 0; theta < 2 * Mathf.PI; i++)
        {
            arr[i] = new Vector3(center.x + Mathf.Cos(theta) * radius, center.y + Mathf.Sin(theta) * radius, center.z);
            theta += step;
        }
    }

    /// <summary>
    /// Finds the contiguous LineSegment [v0, v1] on canditates that Vector3 `to` is closest to
    /// </sumamry>
    public static void ClosestSegment(Vector3 subject, Vector3[] candidates, out Vector3 closest, out Vector3 secondClosest)
    {
        float minimum = float.MaxValue;
        int index = 0;
        for (int i = 0; i < candidates.Length; i++)
        {
            Vector3 candidate = candidates[i];
            float dist = Vector3.Distance(subject, candidate);
            if (dist < minimum)
            {
                minimum = dist;
                index = i;
            }
        }
        closest = candidates[index];
        Vector3[] potential = new Vector3[2];
        if (index + 1 >= candidates.Length)
        {
            potential[0] = candidates[index - 1];
            potential[1] = candidates[0];
            // return [arrayOfPoints[index], arrayOfPoints[index - 1]];
        }
        else if (index == 0)
        {
            potential[0] = candidates[candidates.Length - 1];
            potential[1] = candidates[0];
        }
        else
        {
            potential[0] = candidates[index - 1];
            potential[1] = candidates[index + 1];
        }
        minimum = float.MaxValue;
        secondClosest = potential[0];
        for (int i = 0; i < 2; i++)
        {
            Vector3 point = potential[i];
            float dist = Vector3.Distance(subject, point);
            if (dist < minimum)
            {
                minimum = dist;
                secondClosest = potential[i];
            }
        }
    }
}