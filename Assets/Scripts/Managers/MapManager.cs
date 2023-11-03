using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] List<GameObject> mapZones;
    public List<GameObject> map => mapZones;
    public int currentMapNum = 0;
    GameObject currentMap;

    private void Start()
    {
        currentMap = Instantiate(map[currentMapNum]);
    }
    void SpawnNextZone()
    {
        
    }
}
