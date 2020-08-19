﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMover : MonoBehaviour
{
    public float planetMoveSpeed = 5f;

    //References to planets and orbit lines
    public List<Planet> planets = new List<Planet>();
    public List<OrbitLine> orbitLines = new List<OrbitLine>();

    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    //Where should the planets be in each mode
    private readonly List<Vector3> planetPositionsInDisMode = new List<Vector3>();
    private readonly List<Vector3> planetPositionsInSizeMode = new List<Vector3>();

    private void OnEnable() => Initialise();

    private void Initialise()
    {
        //Cache where planets should be in each mode
        for (int i = 0; i < planets.Count; i++)
        {
            planetPositionsInDisMode.Add(planets[i].Body.transform.position);
            planetPositionsInSizeMode.Add((100 + i * 12) * Vector3.left);
        }
    }

    public IEnumerator LerpPlanetsToPositions(bool switchingToSizeMode)
    {
        List<Vector3> positions = switchingToSizeMode ? planetPositionsInSizeMode : planetPositionsInDisMode;

        //While not all planets are in the correct positions...
        while (!AllPlanetsApproxInPositions(positions))
        {
            //... loop through them...
            for (int i = 0; i < planets.Count; i++)
            {
                Vector3 v = planets[i].Body.transform.position;

                //... and for each one not in the right place...
                if (!v.IsApproximately(positions[i], 0.5f))
                {
                    //... lerp it towards correct position
                    planets[i].Body.transform.position = Vector3.Lerp(v, positions[i], planetMoveSpeed * Time.deltaTime);
                }
            }
            yield return waitForEndOfFrame;
        }
    }

    private bool AllPlanetsApproxInPositions(List<Vector3> positions)
    {
        //Loop through each planet...
        for (int i = 0; i < planets.Count; i++)
        {
            Vector3 v = planets[i].Body.transform.position;

            //... and if one of them is not sufficiently close to target, then not all of them are in correct position
            if (!v.IsApproximately(positions[i], 0.5f))
                return false;
        }

        //Otherwise, all in correct prosition
        return true;
    }
}
