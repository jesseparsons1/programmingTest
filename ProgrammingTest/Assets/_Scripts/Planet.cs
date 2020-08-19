using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planet : MonoBehaviour
{
    public enum Names { Sun, Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto }

    [System.Serializable]
    public class Info
    {
        public Names planetName;
        public float planetTemp;
        public float planetGravityScale;
        public int numMoons;
        public int distanceFromSun;
    }

    [SerializeField]
    private GameObject body = null;
    public GameObject Body => body;
    [SerializeField]
    private int planetIndex = 0;
    [SerializeField]
    private GameObject worldSpaceInfo;
    [SerializeField]
    private TextMeshProUGUI distanceFromSunText;

    public Info MyInfo { get; private set; }

    private void OnEnable() => GameManager.OnCurrentlyViewedPlanetChangedEvent += OnCurrentlyViewedPlanetChanged;

    private void OnDisable() => GameManager.OnCurrentlyViewedPlanetChangedEvent -= OnCurrentlyViewedPlanetChanged;

    private void Start()
    {
        MyInfo = GameManager.instance.PlanetDataHolder.PlanetInfos[planetIndex];
        distanceFromSunText.text = MyInfo.distanceFromSun.AddCommas() + ",000,000 km";
    }

    private void OnCurrentlyViewedPlanetChanged() => worldSpaceInfo.SetActive(GameManager.instance.CurrentlyViewedPlanetIndex == planetIndex);
}
