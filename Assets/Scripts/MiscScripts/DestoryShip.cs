using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryShip : MonoBehaviour
{

    public GameObject brokenShip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coliderColider)
    {
        if(coliderColider.tag == "Planet")
        {
            // Debug.Log(brokenShip.name);
            Instantiate(brokenShip, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
