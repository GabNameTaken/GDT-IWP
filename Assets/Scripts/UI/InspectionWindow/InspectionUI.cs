using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    [Header("Tabs")]
    [SerializeField] GameObject tabsGO;
    [SerializeField] Button tabButtonPrefab;
    [SerializeField] GameObject statsDesc, skillDesc;

    [Header("Status Effects")]
    [SerializeField] StatusIcon statusIconPrefab;
    [SerializeField] GameObject statusContent;

    public void InitTabs(EntityBase inspectEntity)
    {
        //Display name of the entity
        nameText.text = inspectEntity.entity.entityName;

        EmptyTabs();

        //Instantiate tabs
        SetTabs(inspectEntity);

        //Set stat page as default
        DisplayStats(inspectEntity);
    }

    void EmptyTabs()
    {
        //Empty tabs
        if (tabsGO.transform.childCount > 0)
        {
            for (int i = tabsGO.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = tabsGO.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    void SetTabs(EntityBase inspectEntity)
    {
        for (int i = 0; i < inspectEntity.skillSet.SkillDict.Count + 1; i++)
        {
            GameObject go = Instantiate(tabButtonPrefab.gameObject, tabsGO.transform);
            SetTabName(go.transform, i);
            if (i > 0)
            {
                SKILL_CODE selectedSkillCode = inspectEntity.skillSet.SkillDict.Keys.ElementAt(i - 1);
                go.GetComponent<Button>().onClick.AddListener(delegate { DisplaySkills(inspectEntity, selectedSkillCode); });
            }
            else
                go.GetComponent<Button>().onClick.AddListener(delegate { DisplayStats(inspectEntity); });
        }
    }

    void SetTabName(Transform tab, int num)
    {
        TMP_Text nameText = tab.GetChild(0).GetComponent<TMP_Text>();
        switch (num)
        {
            case 0:
                nameText.text = "Stats";
                break;
            case 1:
                nameText.text = "S1";
                break;
            case 2:
                nameText.text = "S2";
                break;
            case 3:
                nameText.text = "S3";
                break;
            case 4:
                nameText.text = "Passive";
                break;
            case 5:
                nameText.text = "Passive";
                break;
        }
    }

    [Header("Description Settings")]
    [SerializeField] TMP_Text hptext;
    [SerializeField] TMP_Text defText, atkText, spdText, critText, cdmgText, turnMeterText;
    [SerializeField] GameObject statusEffectsUI;
    [SerializeField] TMP_Text skillNameText, skillDescText, skillCooldownText, skillCostText;

    public void DisplayStats(EntityBase inspectEntity)
    {
        hptext.text = "Health: " + inspectEntity.trueStats.health + "/" + inspectEntity.trueStats.maxHealth;
        defText.text = "Defense: " + inspectEntity.trueStats.defense;
        atkText.text = "Attack: " + inspectEntity.trueStats.attack;
        spdText.text = "Speed: " + inspectEntity.trueStats.speed;
        critText.text = "Critical Rate: " + inspectEntity.trueStats.critRate + "%";
        cdmgText.text = "Critical Hit Damage: " + inspectEntity.trueStats.critDMG + "%";
        turnMeterText.text = "Turn Meter: " + Mathf.Round(inspectEntity.TurnMeter) + "%";

        DisplayStatus(inspectEntity);

        skillDesc.SetActive(false);
        statsDesc.SetActive(true);
    }

    void DisplayStatus(EntityBase inspectEntity)
    {
        if (statusContent.transform.childCount > 0)
        {
            for (int i = statusContent.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = statusContent.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        foreach (StatusEffect status in inspectEntity.statusEffectList)
        {
            StatusIcon icon = Instantiate(statusIconPrefab, statusContent.transform);
            icon.GetComponent<Image>().sprite = status.StatusEffectData.icon;
            status.icon = icon;
            icon.GetComponent<StatusIcon>().InitIcon(status);

            icon.durationText.text = status.remainingDuration.ToString();
        }
    }

    public void DisplaySkills(EntityBase inspectEntity, SKILL_CODE code)
    {
        Skill skill = inspectEntity.skillSet.SkillDict[code];
        skillNameText.text = skill.skillName;
        skillDescText.text = skill.skillDesc;
        skillCooldownText.text = "Cooldown: " + skill.currentCooldown;
        skillCostText.text = "Skill Points: " + skill.skillCost;
        statsDesc.SetActive(false);
        skillDesc.SetActive(true);
    }
}
