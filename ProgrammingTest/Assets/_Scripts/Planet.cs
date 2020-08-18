using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private GameObject body = null;
    public GameObject Body => body;

    [SerializeField]
    private int planetIndex = 0;

    public PlanetData.PlanetInfo Info { get; private set; }

    private void Start()
    {
        Debug.Log(GameManager.instance);
        Info = GameManager.instance.PlanetDataHolder.PlanetInfos[planetIndex];
    }
}
