using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickShowInfo : MonoBehaviour
{
    [SerializeField]
    private Planet myPlanet = null;
    [SerializeField]
    private Animator animator = null;

    private void OnMouseDown()
    {
        //Update the currently displayed info
        if (GameManager.instance.IsInteractable)
        {
            GameManager.instance.CurDisplayedInfo = myPlanet.MyInfo;
            animator.SetTrigger("Click");
        }
    }
}
