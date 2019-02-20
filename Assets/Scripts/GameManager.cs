using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Planet { get; private set; } 
    public GameObject Base { get; private set; }
    public List<GameObject> Ants { get; private set; }

    private void Awake()
    {
        Planet = Instantiate(Resources.Load("Planet")) as GameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
