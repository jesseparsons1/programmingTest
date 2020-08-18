using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;
    [SerializeField]
    private LineRenderer lineRenderer;

    private float distance;
    public float Distance
    {
        get => distance;
        set
        {
            //If distance changes, we need to update orbit line
            if (value != distance)
            {
                distance = Mathf.Max(0, value);
                UpdateLine();
            }
        }
    }

    [SerializeField]
    private int numSectors;

    float sectorAngle;
    Vector3[] linePoints;

    private void OnEnable()
    {
        Initialise();
        UpdateLine();
    }

    private void Initialise()
    {
        //Since the number of segments will not change at run time, we can cache several values
        lineRenderer.positionCount = numSectors + 1;
        sectorAngle = (float) 360 / numSectors;
        linePoints = new Vector3[numSectors + 1];
    }

    private void UpdateLine()
    {
        //Recalculate line points
        for (int i = 0; i <= numSectors; i++)
            linePoints[i] = Quaternion.Euler(0, i * sectorAngle, 0) * Vector3.left * Distance;

        //Set line points
        lineRenderer.SetPositions(linePoints);
    }

    //Update distance every frame
    private void Update() => Distance = planet.transform.position.magnitude;
}
