using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int MaxConcurrentAnts = 10;
    public Planet Planet { get; private set; } 
    public Base Base { get; private set; }
    private Light _sun;
    public List<Ant> AntPool;
    public Transform AntSpawn1;
    private float timeSinceLastAntSpawned = 0;

    private void Awake()
    {
        Planet = Utilities.SelectPlanet(gameObject);
        AntPool = new List<Ant>();
        _sun = FindObjectOfType<Light>();
        InitAntPool();
    }

    void InitAntPool()
    {
        for (int i = 0; i < MaxConcurrentAnts; i++) {
            var ant = (Instantiate(Resources.Load("Ant")) as GameObject).GetComponent<Ant>();
            ant.gameObject.SetActive(false);
            ant.GameManager = this;
            AntPool.Add(ant);
        }
    }

    void SpawnAnt(Vector3 pos)
    {
        for (int i = 0; i < MaxConcurrentAnts; i++) {
            if (!AntPool[i].gameObject.activeInHierarchy) {
                AntPool[i].gameObject.SetActive(true);
                AntPool[i].transform.position = pos;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _sun.transform.RotateAround(Planet.transform.position, Vector3.up, Time.deltaTime); // make it look like time passing with moving sun (geocentric theory 4 lyfe), fuck copernicus!
        if (timeSinceLastAntSpawned > 2) {
            SpawnAnt(AntSpawn1.position);
            timeSinceLastAntSpawned = 0;
        } else {
            timeSinceLastAntSpawned += Time.deltaTime;
        }
    }
}
