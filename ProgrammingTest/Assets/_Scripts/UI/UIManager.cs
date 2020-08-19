using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentMode = null;
    [SerializeField]
    private TextMeshProUGUI currentPlanetInfo = null;
    [SerializeField]
    private Button switchModeButton = null;
    [SerializeField]
    private GameObject viewPlanetButtonParent = null;
    [SerializeField]
    private List<Button> viewPlanetButtons = new List<Button>();

    private void OnEnable() => GameManager.OnCurrentInfoUpdatedEvent += OnCurrentInfoChanged;

    private void OnDisable() => GameManager.OnCurrentInfoUpdatedEvent -= OnCurrentInfoChanged;

    private void Update()
    {
        switchModeButton.interactable = GameManager.instance.IsInteractable;

        foreach (Button button in viewPlanetButtons)
            button.interactable = GameManager.instance.IsInteractable;
    }

    private void ClearPlanetInfoText() => currentPlanetInfo.text = "Click on a planet to find out more!";

    private void OnCurrentInfoChanged()
    {
        //currentPlanetInfo.text =   "Name: " + GameManager.instance.CurDisplayedInfo.planetName +"\n" +
        //                        "Temperature (F): " + GameManager.instance.CurDisplayedInfo.planetTemp + "\n" +
        //                        "Surface Gravity (m/s^2): " + GameManager.instance.CurDisplayedInfo.planetGravityScale + "\n" +
        //                        "Number of Moons: " + GameManager.instance.CurDisplayedInfo.numMoons;
        currentPlanetInfo.text = GameManager.instance.CurDisplayedInfo.description.text;
    }

    #region Buttons

    public void OnMercuryButtonDown() => ViewPlanet(0);

    public void OnVenusButtonDown() => ViewPlanet(1);

    public void OnEarthButtonDown() => ViewPlanet(2);

    public void OnMarsButtonDown() => ViewPlanet(3);

    public void OnJupiterButtonDown() => ViewPlanet(4);

    public void OnSaturnButtonDown() => ViewPlanet(5);

    public void OnUranusButtonDown() => ViewPlanet(6);

    public void OnNeptuneButtonDown() => ViewPlanet(7);

    private void ViewPlanet(int indexOfPlanetToView)
    {
        if (GameManager.instance.IsInteractable)
        {
            if (GameManager.instance.CurrentlyViewedPlanetIndex != indexOfPlanetToView)
            {
                ClearPlanetInfoText();
                StartCoroutine(GameManager.instance.ViewPlanet(indexOfPlanetToView));
            }
        }
    }

    public void OnSwitchModeButtonDown()
    {
        ClearPlanetInfoText();

        bool switchingToDistanceMode = GameManager.instance.CurMode == 1;

        currentMode.text = switchingToDistanceMode ? "DISTANCE MODE" : "SIZE MODE";
        viewPlanetButtonParent.SetActive(switchingToDistanceMode);

        if (GameManager.instance.IsInteractable)
            StartCoroutine(GameManager.instance.ToggleMode());
    }

    #endregion
}
