using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickShowInfo : MonoBehaviour
{
    [SerializeField]
    private Planet myPlanet = null;

    private void OnMouseDown()
    {
        if (GameManager.instance.isInteractable)
            UIManager.instance.CurDisplayedInfo = myPlanet.Info;
    }
}
