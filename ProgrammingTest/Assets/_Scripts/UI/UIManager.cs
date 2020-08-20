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

    private void OnEnable()
    {
        GameManager.OnCurrentInfoUpdatedEvent += OnCurrentInfoChanged;
        GameManager.OnGameInteractabilityToggledEvent += OnGameInteractabilityToggled;
    }

    private void OnDisable()
    {
        GameManager.OnCurrentInfoUpdatedEvent -= OnCurrentInfoChanged;
        GameManager.OnGameInteractabilityToggledEvent -= OnGameInteractabilityToggled;
    }

    //Acts like a view: responds to changes in the data held in GameManager
    private void OnCurrentInfoChanged() => currentPlanetInfo.text = GameManager.instance.CurDisplayedInfo.description.text;
    private void OnGameInteractabilityToggled()
    {
        switchModeButton.interactable = GameManager.instance.IsInteractable;

        foreach (Button button in viewPlanetButtons)
            button.interactable = GameManager.instance.IsInteractable;
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
            //If a planet has been selected that isn't the one currently being viewed, then change to view that one instead
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

        //Depending on which mode we are switching to, update relevant UI elements
        bool switchingToDistanceMode = GameManager.instance.CurMode == 1;

        currentMode.text = switchingToDistanceMode ? "DISTANCE MODE" : "SIZE MODE";
        viewPlanetButtonParent.SetActive(switchingToDistanceMode);

        if (GameManager.instance.IsInteractable)
            StartCoroutine(GameManager.instance.ToggleMode());
    }

    #endregion

    private void ClearPlanetInfoText() => currentPlanetInfo.text = "Click on a planet to find out more!";
}
