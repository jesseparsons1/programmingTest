using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    [SerializeField]
    private GameObject planet = null;
    [SerializeField]
    private LineRenderer lineRenderer = null;
    [SerializeField]
    private int numSectors = 0;

    private float distance;
    public float Distance
    {
        get => distance;
        set
        {
            //If distance changes, we need to update the line
            if (value != distance)
            {
                distance = Mathf.Max(0, value);
                UpdateLine();
            }
        }
    }

    private float sectorAngle;

    private void OnEnable() => Initialise();

    private void Initialise()
    {
        //Since the number of segments will not change at run time...
        //... we can cache the sector angle and initialise lineRenderer to have set number of positions
        lineRenderer.positionCount = numSectors + 1;
        sectorAngle = (float) 360 / numSectors;
        Distance = planet.transform.position.magnitude;
    }

    //Update distance every frame
    private void Update() => Distance = planet.transform.position.magnitude;

    private void UpdateLine()
    {
        //Recalculate line points
        for (int i = 0; i <= numSectors; i++)
            lineRenderer.SetPosition(i, Quaternion.Euler(0, i * sectorAngle, 0) * Vector3.left * Distance);
    }
}
