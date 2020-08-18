using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    public GameObject planet;
    public LineRenderer lineRenderer;

    [SerializeField]
    private float distance;
    public float Distance
    {
        get => distance;
        set
        {
            if (value != distance)
            {
                distance = Mathf.Max(0, value);
                UpdateLine();
            }
            
        }
    }

    [SerializeField]
    private int numSegments;
    public int NumSegments
    {
        get => numSegments;
        set
        {
            numSegments = Mathf.Max(1, value);
            UpdateLine();
        }
    }

    void OnEnable()
    {
        UpdateLine();
    }

    private void Update()
    {
        Distance = planet.transform.position.magnitude;
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = NumSegments + 1;

        float theta = (float) 360 / NumSegments;

        Vector3[] linePoints = new Vector3[NumSegments + 1];
        Vector3 unitVecToPlanet = planet.transform.position.normalized;

        for (int i = 0; i <= numSegments; i++)
        {
            Quaternion rot = Quaternion.Euler(0, i * theta, 0);
            Vector3 newPoint = rot * unitVecToPlanet * distance;
            linePoints[i] = newPoint;
        }

        lineRenderer.SetPositions(linePoints);
    }
}
