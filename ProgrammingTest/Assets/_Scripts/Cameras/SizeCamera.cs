using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject cam = null;

    private void OnEnable() => GameManager.OnToggleModeEvent += OnToggleMode;

    private void OnDisable() => GameManager.OnToggleModeEvent -= OnToggleMode;

    //When the mode changes, camera sets itself active accordingly
    private void OnToggleMode(bool switchingToSizeMode) => cam.SetActive(switchingToSizeMode);
}
