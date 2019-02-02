using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlanetaryAttractor : MonoBehaviour
{
    public float Gravity = -9.8f; // near surface approximation

    void Start()
    {
        
    }
    public void Attract(PlanetaryAttractee attractee)
    {
        // Quaternion targetRotation = Quaternion.FromToRotation(attractee.transform.up, normalizedRelPosVec) * attractee.transform.rotation;
        // attractee.transform.rotation = Quaternion.Slerp(attractee.transform.rotation, targetRotation, 50 * Time.deltaTime);

        // to correct for adding forces while selected
        if(attractee.gravityFlag)
        {
            Vector3 relPosition = attractee.rb.position - transform.position;
            float distance = relPosition.magnitude;
            float mass = attractee.rb.mass;
            float forceMagnitude = mass * Gravity;
            // float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = relPosition.normalized * forceMagnitude;
            attractee.rb.AddForce(force, ForceMode.Force); // continuous acceleration
        }
    }
}