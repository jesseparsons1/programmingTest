using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI currentModeText = null;
    [SerializeField]
    private TextMeshProUGUI planetInfoText = null;
    [SerializeField]
    private Button switchModeButton = null;
    [SerializeField]
    private GameObject viewPlanetButtonParent = null;
    [SerializeField]
    private List<Button> viewPlanetButtons = new List<Button>();

    private PlanetData.PlanetInfo curDisplayedInfo;
    public PlanetData.PlanetInfo CurDisplayedInfo
    {
        get => curDisplayedInfo;
        set
        {
            curDisplayedInfo = value;
            UpdatePlanetInfoText();
        }
    }

    private void Update()
    {
        //Set buttons interactive or uninteractive based on global variable
        switchModeButton.interactable = GameManager.instance.isInteractable;

        foreach (Button button in viewPlanetButtons)
            button.interactable = GameManager.instance.isInteractable;

    }

    private void ClearPlanetInfoText() => planetInfoText.text = "Click on a planet to find out more!";

    private void UpdatePlanetInfoText()
    {
        planetInfoText.text =   "Name: " + CurDisplayedInfo.planetName +"\n" +
                                "Temperature (F): " + CurDisplayedInfo.planetTemp + "\n" +
                                "Surface Gravity (m/s^2): " + CurDisplayedInfo.planetGravityScale + "\n" +
                                "Number of Moons: " + CurDisplayedInfo.numMoons; 
    }

    public void OnMercuryButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(0));
    }

    public void OnVenusButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(1));
    }

    public void OnEarthButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(2));
    }

    public void OnMarsButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(3));
    }

    public void OnJupiterButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(4));
    }

    public void OnSaturnButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(5));
    }

    public void OnUranusButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(6));
    }

    public void OnNeptuneButtonDown()
    {
        ClearPlanetInfoText();
        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.MoveDistanceCameraToViewPlanetNo(7));
    }

    public void OnSwitchModeButtonDown()
    {
        ClearPlanetInfoText();

        bool switchingToDistanceMode = GameManager.instance.Mode == 1;

        currentModeText.text = switchingToDistanceMode ? "DISTANCE MODE" : "SIZE MODE";
        viewPlanetButtonParent.SetActive(switchingToDistanceMode);

        if (GameManager.instance.isInteractable)
            StartCoroutine(GameManager.instance.ToggleMode());
    }
}
