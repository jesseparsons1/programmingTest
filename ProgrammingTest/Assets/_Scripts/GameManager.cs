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
    public DistanceCamera DistanceCamera => distanceCamera;

    #region Events

    public delegate void OnGameInteractabilityToggled();
    public static event OnGameInteractabilityToggled OnGameInteractabilityToggledEvent;

    public delegate void OnCurrentInfoUpdated();
    public static event OnCurrentInfoUpdated OnCurrentInfoUpdatedEvent;

    public delegate void OnCurrentlyViewedPlanetChanged();
    public static event OnCurrentlyViewedPlanetChanged OnCurrentlyViewedPlanetChangedEvent;

    public delegate void OnToggleMode(bool switchingToSizeMode);
    public static event OnToggleMode OnToggleModeEvent;

    #endregion

    //The current info being displayed about the planets, in response to the player clicking on them
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

    //The index 0-7 of the planet currently being viewed by the distance camera, values outside of this range mean no planet is being viewed 
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

    //Determines whether or not buttons and planets are interactable at a given moment in time
    private bool isInteractable = true;
    public bool IsInteractable
    {
        get => isInteractable;
        private set
        {
            bool wasInteractable = isInteractable;
            isInteractable = value;
            if (wasInteractable != isInteractable)
            {
                OnGameInteractabilityToggledEvent?.Invoke();
            }
        }
    }

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

        yield return StartCoroutine(distanceCamera.ChangeView(i));

        CurrentlyViewedPlanetIndex = i;

        //Reenable game interactivity
        IsInteractable = true;
    }

    int lastViewedPlanetIndex;

    public IEnumerator ToggleMode()
    {
        //Set game uninteractive
        IsInteractable = false;

        //If we are currently in distance mode, then we are switching to size mode
        bool switchingToSizeMode = CurMode == 0;

        if (switchingToSizeMode)
        {
            //Since no planet is currently being viewed, we assign a negative value and store last viewed planet index
            lastViewedPlanetIndex = CurrentlyViewedPlanetIndex;
            CurrentlyViewedPlanetIndex = -1;
        }
        else //If switching back to distance mode...
        {
            //... return to viewing last viewed planet
            CurrentlyViewedPlanetIndex = lastViewedPlanetIndex;
        }
            
        //Toggle mode
        CurMode = switchingToSizeMode ? 1 : 0;

        OnToggleModeEvent?.Invoke(switchingToSizeMode);

        yield return StartCoroutine(planetMover.LerpPlanetsToPositions(switchingToSizeMode));

        //Reenable game interactivity
        IsInteractable = true;
    }

    //Retrieve planet from planet mover by index
    public Planet GetPlanet(int i) => planetMover.Planets[i];
}
