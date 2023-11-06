using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamManager : MonoBehaviour
{
    public static PlayerTeamManager Instance { get; private set; }
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

    public List<GameObject> teamPrefabs;

    //To do reaaranging function for team setup
}
