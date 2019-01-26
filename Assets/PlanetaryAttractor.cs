using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlanetaryAttractor : MonoBehaviour
{
    public float Gravity = -9.8f;


    public void Attract(PlanetaryAttractee attractee)
    {
        Vector3 relativePositionVector = (attractee.transform.position - transform.position).normalized;

        // attractee.rigidBody.AddForce(relativePositionVector * Gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(attractee.transform.up, relativePositionVector) * attractee.transform.rotation;
        attractee.transform.rotation = Quaternion.Slerp(attractee.transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}