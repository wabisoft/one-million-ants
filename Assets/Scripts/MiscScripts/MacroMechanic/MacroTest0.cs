using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// basic idea: if a specific object collides with another object and has right
// permutation -- destroy and replace with correct prefab


public class MacroTest0 : MonoBehaviour
{

    public GameObject permutationModel;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wing1" )
        {
            Instantiate(permutationModel, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
