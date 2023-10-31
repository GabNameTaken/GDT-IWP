using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance { get; private set; }
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

    [SerializeField] List<HealthUI> teamHealth;

    public void AddPlayerTeamStats(List<PlayableCharacter> playerTeam)
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            teamHealth[i].character = playerTeam[i];
            teamHealth[i].SetUpUI();
        }
    }

    public void UpdatePlayerTeamHealth()
    {
        for (int i = 0; i < teamHealth.Count; i++)
        {
            if (teamHealth[i] == null)
                continue;
            teamHealth[i].UpdateHealthUI();
        }
    }
}
