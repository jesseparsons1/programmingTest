using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickShowInfo : MonoBehaviour
{
    [SerializeField]
    private Planet myPlanet;

    //bool isInitialised = false;
    //PlanetData.PlanetInfo myInfo;

    private void OnMouseDown()
    {
        if (GameManager.instance.isInteractable)
        {
            UIManager.instance.CurDisplayedInfo = myPlanet.myInfo;
        }
    }
}
