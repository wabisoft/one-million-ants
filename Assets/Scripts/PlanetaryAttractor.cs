using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlanetaryAttractor : MonoBehaviour
{
    public float Gravity = -9.8f;

    public void Attract(PlanetaryAttractee attractee)
    {
        Vector3 normalizedRelPosVec = (attractee.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(attractee.transform.up, normalizedRelPosVec) * attractee.transform.rotation;
        attractee.transform.rotation = Quaternion.Slerp(attractee.transform.rotation, targetRotation, 50 * Time.deltaTime);

        // to correct for adding forces while selected
        if(attractee.gravityFlag)
        {
            float mass = attractee.rb.mass;
            // gravity near surface = mg
            attractee.rb.AddForce(normalizedRelPosVec * mass * Gravity);
        }
        
    }
}