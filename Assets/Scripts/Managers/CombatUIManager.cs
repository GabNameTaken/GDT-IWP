using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] List<StatusEffectUI> teamStatus;
    public void SetUpPlayerUI(List<PlayableCharacter> playerTeam)
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            teamHealth[i].character = playerTeam[i];
            teamHealth[i].SetUpUI();
            playerTeam[i].statusEffectUI = teamStatus[i];
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

    [SerializeField] Transform skillSetUI;
    string[] skillKeyBindTexts = { "S1", "S2", "S3" };
    public void DisplaySkillCooldown(SkillSet skillSet)
    {
        for (int i = 0; i < skillSetUI.childCount; i++)
        {
            if (skillSet.SkillDict.ContainsKey((SKILL_CODE)i) && skillSet.SkillDict[(SKILL_CODE)i].currentCooldown > 0)
                skillSetUI.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = skillSet.SkillDict[(SKILL_CODE)i].currentCooldown.ToString();
            else
                skillSetUI.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = skillKeyBindTexts[i];
        }
    }
}
