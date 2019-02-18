using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Planet Planet { get; private set; } 
    public Base Base { get; private set; }

    private void Awake()
    {
        Planet = Utilities.SelectPlanet(gameObject);
        Base = Instantiate(Base);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
