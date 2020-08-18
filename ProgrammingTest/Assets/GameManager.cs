using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float planetLerpSpeed = 5f;

    public Camera sizeModeCamera;
    public Camera distanceModeCamera;

    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private readonly List<Vector3> positionsInDistanceMode = new List<Vector3>();
    private readonly List<Vector3> positionsInSizeMode = new List<Vector3>();

    public List<Planet> planets = new List<Planet>();
    public List<OrbitLine> orbitLines = new List<OrbitLine>();

    public bool isInteractable = true;

    //0 or 1, represents current game mode, distance or size respectively
    private int mode = 0;
    public int Mode
    {
        get => mode;
        set
        {
            mode = (int)Mathf.Clamp01(value);
        }
    }

    private void OnEnable() => Initialise();

    private void Initialise()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            positionsInDistanceMode.Add(planets[i].planetBody.transform.position);
            positionsInSizeMode.Add((100 + i * 12) * Vector3.left);
        }
    }

    public IEnumerator ToggleMode()
    {
        isInteractable = false;

        distanceModeCamera.gameObject.SetActive(Mode == 1);
        sizeModeCamera.gameObject.SetActive(Mode == 0);

        yield return StartCoroutine(LerpPlanetsToPositions(Mode == 0 ? positionsInSizeMode : positionsInDistanceMode));

        Mode = (Mode + 1) % 2;

        isInteractable = true;
    }

    public IEnumerator LerpPlanetsToPositions(List<Vector3> positions)
    {
        while (!AllPlanetsApproxInPositions(positions))
        {
            for (int i = 0; i < planets.Count; i++)
            {
                Vector3 v = planets[i].planetBody.transform.position;

                if (!v.IsApproximately(positions[i]))
                {
                    planets[i].planetBody.transform.position = Vector3.Lerp(v, positions[i], planetLerpSpeed * Time.deltaTime);
                }
            }
            yield return waitForEndOfFrame;
        }
    }

    private bool AllPlanetsApproxInPositions(List<Vector3> positions)
    {
        for (int i = 0; i < planets.Count; i++)
        {
            Vector3 v = planets[i].planetBody.transform.position;

            if (!v.IsApproximately(positions[i]))
                return false;
        }
        return true;
    }
}
