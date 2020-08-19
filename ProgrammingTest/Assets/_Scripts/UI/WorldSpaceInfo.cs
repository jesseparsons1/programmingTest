﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldSpaceInfo : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI distanceFromSunText;
    public TextMeshProUGUI DistanceFromSunText => distanceFromSunText;

    public void ToggleInfo(bool on)
    {
        canvas.gameObject.SetActive(on);
    }

    private void Update()
    {
        if (canvas.gameObject.activeInHierarchy)
        {
            canvas.transform.LookAt(GameManager.instance.DistanceCamera.Camera.transform);
            canvas.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
        }
    }
}