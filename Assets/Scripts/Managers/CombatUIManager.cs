using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using Common.DesignPatterns;

public class CombatUIManager : Singleton<CombatUIManager>
{
    [Header("Team Info UI")]
    public List<EntityInfoUI> teamInfoUIs;
    [SerializeField] List<StatusEffectUI> teamStatus;

    [Header("Boss UI")]
    public EntityInfoUI bossInfoUI;
    [SerializeField] StatusEffectUI bossStatusUI;

    [Header("Turn Order UI")]
    [SerializeField] Slider turnSlider;
    [SerializeField] RectTransform turnSliderHandle;

    public void SetUpPlayerUI(List<PlayableCharacter> playerTeam)
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            teamInfoUIs[i].character = playerTeam[i];
            teamInfoUIs[i].SetUpUI();
            playerTeam[i].statusEffectUI = teamStatus[i];
        }
    }

    public void UpdateHealth(EntityBase entity)
    {
        if (CombatManager.Instance.entitiesOnField.Contains(entity))
        {
            EntityInfoUI infoUI = teamInfoUIs.FirstOrDefault(info => info.character == entity);
            infoUI.UpdateHealthUI();
        }
    }

    [Header("Player's Turn UI")]
    [SerializeField] List<Transform> skillSetUI;
    string[] skillKeyBindTexts = { "S1", "S2", "S3" };
    public void DisplaySkillCooldown(SkillSet skillSet)
    {
        for (int i = 0; i < skillSetUI.Count; i++)
        {
            if (skillSet.SkillDict.ContainsKey((SKILL_CODE)i) && skillSet.SkillDict[(SKILL_CODE)i].currentCooldown > 0)
                skillSetUI[i].GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = skillSet.SkillDict[(SKILL_CODE)i].currentCooldown.ToString();
            else
                skillSetUI[i].GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = skillKeyBindTexts[i];
        }
    }

    public void DisplaySelectedSkill(SKILL_CODE code)
    {
        DeselectSkills();
        EventSystem.current.SetSelectedGameObject(skillSetUI[(int)code].GetChild(0).gameObject);
    }

    public void DeselectSkills()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public Vector2 GetSliderButtonPosition(float turnMeter)
    {
        turnSlider.value = turnMeter;
        return turnSliderHandle.transform.position;
    }
}
