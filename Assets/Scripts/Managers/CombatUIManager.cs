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
    public List<EntityInfoUI> infoUIs;

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
            infoUIs[i].character = playerTeam[i];
            infoUIs[i].SetUpUI();
            playerTeam[i].entityInfoUI = infoUIs[i];
        }
    }

    public void UpdateHealth(EntityBase entity)
    {
        if (CombatManager.Instance.entitiesOnField.Contains(entity))
        {
            EntityInfoUI infoUI = infoUIs.FirstOrDefault(info => info.character == entity);
            infoUI.UpdateHealthUI();
        }
    }

    [Header("Wave Number Display")]
    [SerializeField] TMP_Text waveText;

    public void SetWaveNumber(int num, int total)
    {
        waveText.text = "Wave: " + num + " / " + total;
    }

    [Header("Player's Turn UI")]
    public SkillUI skillUI;
    [SerializeField] List<Transform> skillSetUI;
    [SerializeField] TMP_Text controlsText;
    string[] skillKeyBindTexts = { "S1", "S2", "S3" };
    public void DisplaySkillCooldown(SkillSet skillSet)
    {
        displayTurnControls();
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
        displayTargetControls();
        EventSystem.current.SetSelectedGameObject(skillSetUI[(int)code].GetChild(0).gameObject);
    }

    public void DeselectSkills()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void displayTargetControls()
    {
        controlsText.fontSize = 18;
        controlsText.text = "ARROW KEYS|A D : Target SPACE : Confirm";
    }

    void displayTurnControls()
    {
        controlsText.fontSize = 24;
        controlsText.text = "TAB : Consume Ether SPACE : Confirm";
    }

    public Vector2 GetSliderButtonPosition(float turnMeter)
    {
        turnSlider.value = turnMeter;
        return turnSliderHandle.transform.position;
    }

    [Header("Damage numbers")]
    [SerializeField] NumberCounter damageNumber;
    [SerializeField] Color normalDMGColor;
    [SerializeField] Color critDMGColor;

    public void ShowDMGNumbers(float damage, bool crit)
    {
        if (crit)
            damageNumber.StartCount(Mathf.RoundToInt(damage), critDMGColor);
        else
            damageNumber.StartCount(Mathf.RoundToInt(damage), normalDMGColor);
    }
}
