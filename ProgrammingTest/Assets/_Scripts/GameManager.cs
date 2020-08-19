using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlanetDataHolder planetDataHolder = null;
    public PlanetDataHolder PlanetDataHolder => planetDataHolder;
    [SerializeField]
    private PlanetMover planetMover = null;
    public PlanetMover PlanetMover => planetMover;
    [SerializeField]
    private DistanceCamera distanceCamera = null;

    #region Events

    public delegate void OnCurrentInfoUpdated();
    public static event OnCurrentInfoUpdated OnCurrentInfoUpdatedEvent;

    public delegate void OnCurrentlyViewedPlanetChanged();
    public static event OnCurrentlyViewedPlanetChanged OnCurrentlyViewedPlanetChangedEvent;

    public delegate void OnToggleMode(bool switchingToSizeMode);
    public static event OnToggleMode OnToggleModeEvent;

    #endregion

    private Planet.Info curDisplayedInfo;
    public Planet.Info CurDisplayedInfo
    {
        get => curDisplayedInfo;
        set
        {
            curDisplayedInfo = value;
            OnCurrentInfoUpdatedEvent?.Invoke();
        }
    }

    private int currentlyViewedPlanetIndex;
    public int CurrentlyViewedPlanetIndex
    {
        get => currentlyViewedPlanetIndex;
        set
        {
            currentlyViewedPlanetIndex = value;
            OnCurrentlyViewedPlanetChangedEvent?.Invoke();
        }
    }

    public bool IsInteractable { get; private set; }  = true;

    //Can take values 0 or 1 representing current game mode, distance or size, respectively
    private int curMode = 0;
    public int CurMode
    {
        get => curMode;
        set
        {
            curMode = (int)Mathf.Clamp01(value);
        }
    }

    private void Start()
    {
        //After behaviours have subscribed to events, we can initialise values
        CurrentlyViewedPlanetIndex = 0;
    }

    public IEnumerator ViewPlanet(int i)
    {
        //Set game uninteractive
        IsInteractable = false;

        //Since no planet is currently being viewed, we assign a negative value
        CurrentlyViewedPlanetIndex = -1;

        yield return StartCoroutine(distanceCamera.MoveToView(i));

        //Reenable game interactivity
        IsInteractable = true;
    }

    public IEnumerator ToggleMode()
    {
        //Set game uninteractive
        IsInteractable = false;

        //If we are currently in distance mode, then we are switching to size mode
        bool switchingToSizeMode = CurMode == 0;

        OnToggleModeEvent?.Invoke(switchingToSizeMode);

        yield return StartCoroutine(planetMover.LerpPlanetsToPositions(switchingToSizeMode));

        //Toggle mode
        CurMode = (CurMode + 1) % 2;

        //Reenable game interactivity
        IsInteractable = true;
    }
}
