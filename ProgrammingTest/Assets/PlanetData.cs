using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetData : MonoBehaviour
{
    public enum PlanetNames { Sun, Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto}

    [System.Serializable]
    public class PlanetInfo
    {
        public PlanetNames planetName;
        public float planetTemp;
        public float planetGravityScale;
        public int numMoons;
    }

    public List<PlanetInfo> planetInfos = new List<PlanetInfo>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
