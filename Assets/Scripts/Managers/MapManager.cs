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
    public GameObject currentMap;

    public GameObject battleground { get; private set; }

    private void Start()
    {
        SetMap();
    }

    public void NextMap()
    {
        Destroy(currentMap);
        if (currentMapNum + 1 < mapZones.Count)
        {
            currentMapNum += 1;
            SetMap();
        }
    }

    public void SetMap()
    {
        currentMap = Instantiate(map[currentMapNum]);
        GameController.Instance.CombatSetup(currentMap.GetComponent<CombatZone>());
        battleground = currentMap.GetComponent<CombatZone>().battleground;
    }
}
