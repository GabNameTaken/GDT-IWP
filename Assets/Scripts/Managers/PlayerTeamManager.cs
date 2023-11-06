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
    public List<PlayableCharacter> playerTeam { get; private set; }

    private void Start()
    {
        playerTeam = new();
        SpawnTeam();
    }

    void SpawnTeam()
    {
        playerTeam.Clear();
        foreach (GameObject prefab in teamPrefabs)
        {
            GameObject character = Instantiate(prefab);
            playerTeam.Add(character.GetComponent<PlayableCharacter>());
        }
    }
    //To do reaaranging function for team setup
}
