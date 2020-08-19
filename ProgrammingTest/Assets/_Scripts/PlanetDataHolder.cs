using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDataHolder : MonoBehaviour
{
    [SerializeField]
    private List<Planet.Info> planetInfos = new List<Planet.Info>();
    public List<Planet.Info> PlanetInfos => planetInfos; 
}
