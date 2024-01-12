using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EntityInfoUI : MonoBehaviour
{
    public EntityBase character;

    [Header("Manual SetUp")]
    [SerializeField] Image sprite;
    [SerializeField] Image elementIcon;
    [SerializeField] Image classIcon;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthText, nameText;
    
    [SerializeField] StatusEffectUI _statusEffectUI;
    public StatusEffectUI statusEffectUI => _statusEffectUI;

    [Header("Enemy UI")]
    Dictionary<SKILL_CODE, GameObject> skillUIs = new();
    [SerializeField] GameObject skillSetUI;
    [SerializeField] GameObject skillUIPrefab;

    private void Start()
    {
        if (character != null)
        {
            CombatUIManager.Instance.infoUIs.Add(this);
            SetUpUI();
        }
    }

    public void SetUpUI()
    {
        if (nameText)
            nameText.text = character.entity.entityName;
        healthSlider.maxValue = character.trueStats.maxHealth;  //Update Max HP
        healthSlider.value = character.trueStats.health;    //Set current HP
        if (healthText)
            healthText.text = Mathf.RoundToInt(character.trueStats.health).ToString();
        if (sprite)
            sprite.sprite = character.entity.sprite;
        if (elementIcon)
            elementIcon.sprite = character.entity.element.elementImage;
        if (classIcon)
            classIcon.sprite = character.entity.classType.classImage;
    }

    public void UpdateHealthUI()
    {
        healthSlider.maxValue = character.trueStats.maxHealth;  //Update Max HP
        healthSlider.value = character.trueStats.health;    //Set current HP
        if (healthText)
            healthText.text = Mathf.RoundToInt(character.trueStats.health).ToString();
    }

    void SetUpSkillUI()
    {
        foreach (Skill skill in character.skillSet.SkillDict.Values)
        {
            if (character.skillSet.SkillDict[SKILL_CODE.S1] == skill)
                continue;
            SKILL_CODE code = character.skillSet.SkillDict.FirstOrDefault(x => x.Value == skill).Key;
            GameObject UI = Instantiate(skillUIPrefab, skillSetUI.transform);
            UI.GetComponent<Slider>().maxValue = character.skillSet.SkillDict[code].cooldown;
            skillUIs.Add(code, UI);
        }
    }

    public void UpdateSkillUI()
    {
        foreach (SKILL_CODE code in skillUIs.Keys)
        {
            skillUIs[code].GetComponent<Slider>().value = character.skillSet.SkillDict[code].currentCooldown;
        }
    }
}
