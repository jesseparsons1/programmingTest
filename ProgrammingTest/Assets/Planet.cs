using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    //public Vector3 originalPosition;
    public GameObject planetBody;
    public PlanetData planetDataHolder;

    public PlanetData.PlanetNames myName;
    public PlanetData.PlanetInfo myInfo;

    private void OnEnable()
    {
        Initialise();
    }

    private void Initialise()
    {
        //originalPosition = planetBody.transform.position;

        //Planet gets its information from planet data holder
        foreach (PlanetData.PlanetInfo planetInfo in planetDataHolder.planetInfos)
        {
            if (myName == planetInfo.planetName)
            {
                myInfo = planetInfo;
            }
        }
    }
}
