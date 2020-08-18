using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlanetData planetDataHolder = null;
    public PlanetData PlanetDataHolder => planetDataHolder;

    public float planetLerpSpeed = 5f;

    public Camera sizeModeCamera;
    public Camera disModeCam;
    public Transform disModeCamRail;
    public float disModeCamMoveSpeed;

    private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    //Where should the planets be in each mode
    private readonly List<Vector3> planetPositionsInDisMode = new List<Vector3>();
    private readonly List<Vector3> planetPositionsInSizeMode = new List<Vector3>();

    public List<Vector3> camPivotPositionsInDisMode = new List<Vector3>();
    public List<Planet> planets = new List<Planet>();
    public List<OrbitLine> orbitLines = new List<OrbitLine>();

    public bool isInteractable = true;

    //Can take values 0 or 1 representing current game mode, distance or size, respectively
    private int mode = 0;
    public int Mode
    {
        get => mode;
        set
        {
            mode = (int)Mathf.Clamp01(value);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Initialise();
    }

    private void Initialise()
    {
        //Cache where planets should be in each mode
        for (int i = 0; i < planets.Count; i++)
        {
            planetPositionsInDisMode.Add(planets[i].Body.transform.position);
            planetPositionsInSizeMode.Add((100 + i * 12) * Vector3.left);
        }
    }

    public IEnumerator MoveDistanceCameraToViewPlanetNo(int i)
    {
        //Set game uninteractive
        isInteractable = false;

        //Get cam rail position for target planet
        Vector3 targetPosition = camPivotPositionsInDisMode[i];

        //While rail is not sufficiently close to target position, lerp it towards it
        while (!disModeCamRail.position.IsApproximately(targetPosition, 0.2f))
        {
            disModeCamRail.position = Vector3.Lerp(disModeCamRail.position, targetPosition, disModeCamMoveSpeed * Time.deltaTime);
            yield return waitForEndOfFrame;
        }

        //Reenable game interactivity
        isInteractable = true;
    }

    public IEnumerator ToggleMode()
    {
        //Set game uninteractive
        isInteractable = false;

        //If we are currently in distance mode, then we are switching to size mode
        bool switchingToSizeMode = Mode == 0;

        //Toggle one camera on and the other off
        disModeCam.gameObject.SetActive(!switchingToSizeMode);
        sizeModeCamera.gameObject.SetActive(switchingToSizeMode);

        yield return StartCoroutine(LerpPlanetsToPositions(switchingToSizeMode ? planetPositionsInSizeMode : planetPositionsInDisMode));

        //Toggle mode
        Mode = (Mode + 1) % 2;

        //Reenable game interactivity
        isInteractable = true;
    }

    private IEnumerator LerpPlanetsToPositions(List<Vector3> positions)
    {
        //While not all planets are in the correct positions...
        while (!AllPlanetsApproxInPositions(positions))
        {
            //... loop through them...
            for (int i = 0; i < planets.Count; i++)
            {
                Vector3 v = planets[i].Body.transform.position;

                //... and for each one not in the right place...
                if (!v.IsApproximately(positions[i]))
                {
                    //... lerp it towards correct position
                    planets[i].Body.transform.position = Vector3.Lerp(v, positions[i], planetLerpSpeed * Time.deltaTime);
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
