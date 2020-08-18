using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI planetInfoText;

    private PlanetData.PlanetInfo curDisplayedInfo;
    public PlanetData.PlanetInfo CurDisplayedInfo
    {
        get => curDisplayedInfo;
        set
        {
            if (curDisplayedInfo != value)
            {
                curDisplayedInfo = value;
                UpdatePlanetInfoText();
            }
        }
    }


    private void UpdatePlanetInfoText()
    {
        planetInfoText.text = "Name: " + CurDisplayedInfo.planetName + "\n" + "Temperature (F): " + CurDisplayedInfo.planetTemp; 
    }

    public void OnSwitchModeButtonDown()
    {
        if (GameManager.instance.isInteractable)
        {
            StartCoroutine(GameManager.instance.ToggleMode());
        }
    }
}
