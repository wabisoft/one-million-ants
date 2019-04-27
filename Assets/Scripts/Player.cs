using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<PlanetaryBody> Selection;
    private Planet _planet;
    public Planet Planet
    {
        get {
            if (!_planet)
            {
                _planet = Utilities.SelectPlanet(gameObject);
            }
            return _planet;
        }
        set {
            _planet = value;
        }
    }


    public void Start()
    {
        Selection = new List<PlanetaryBody>();
    }

    public void OnEnable()
    {
        foreach(var pb in FindObjectsOfType<PlanetaryBody>())
            pb.OnClicked += OnPlanetaryBodyClicked;
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            unit.OnClicked -= OnPlanetaryBodyClicked;
            unit.OnClicked += OnUnitClicked;
        }

        Planet.OnClicked += OnPlanetClicked;
    }

    public void OnPlanetClicked()
    {
        foreach (var pb in Selection)
        {
        }
    }

    public void OnPlanetaryBodyClicked(PlanetaryBody pb)
    {
        Debug.Log("PlanetaryBody clicked");
        Selection.Add(pb);

    }

    public void OnUnitClicked(PlanetaryBody pb)
    {
        Unit unit = pb as Unit;
        Debug.Log("Unit clicked");
    }
}
