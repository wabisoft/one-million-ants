using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int MaxConcurrentUnits = 100;
    public float SpawnTime = 1.0f;
    public Planet Planet { get; private set; } 
    public Base Base { get; private set; }
    private Light _sun;
    public List<Unit> AntPool;
    public Transform AntSpawn1;
    public Ship Ship { get; private set; }
    private float timeSinceLastAntSpawned = 0;

    private void Awake()
    {
        Planet = Utilities.SelectPlanet(gameObject);
        Ship = FindObjectOfType<Ship>();
        Base = FindObjectOfType<Base>();
        AntPool = new List<Unit>();
        _sun = FindObjectOfType<Light>();
        InitAntPool();
    }

    void InitAntPool()
    {
        for (int i = 0; i < MaxConcurrentUnits; i++) {
            var ant = (Instantiate(Resources.Load("SpiralAnt")) as GameObject).GetComponent<Unit>();
            ant.gameObject.SetActive(false);
            // ant.GameManager = this;
            AntPool.Add(ant);
        }
    }

    void SpawnAnt(Vector3 pos)
    {
        for (int i = 0; i < MaxConcurrentUnits; i++) {
            if (!AntPool[i].Active) {
                AntPool[i].Spawn(pos);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _sun.transform.RotateAround(Planet.transform.position, Vector3.up, Time.deltaTime); // make it look like time passing with moving sun (geocentric theory 4 lyfe), fuck copernicus!
        if (timeSinceLastAntSpawned > SpawnTime) {
            SpawnAnt(AntSpawn1.position);
            timeSinceLastAntSpawned = 0;
        } else {
            timeSinceLastAntSpawned += Time.deltaTime;
        }
        

        if (Ship.Complete) {
            Debug.Log("Yay");
        } else if (Base.HP <= 0){
            Debug.Log("You loose");
        }
    }
}
