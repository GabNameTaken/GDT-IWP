using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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
    public List<HealthUI> listOfHealthUIs = new();

    public void SetUpPlayerUI(List<PlayableCharacter> playerTeam)
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            teamHealth[i].character = playerTeam[i];
            teamHealth[i].SetUpUI();
        }
        listOfHealthUIs.AddRange(teamHealth);
    }

    public void UpdateHealth(EntityBase entity)
    {
        if (CombatManager.Instance.entitiesOnField.Contains(entity))
        {
            HealthUI healthUI = listOfHealthUIs.FirstOrDefault(health => health.character == entity);
            healthUI.UpdateHealthUI();
        }
    }
}
