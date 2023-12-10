using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityInfoUI : MonoBehaviour
{
    Slider healthSlider;
    TMP_Text healthText, nameText;
    Image elementIcon;

    public EntityBase character;

    private void Awake()
    {
        if (transform.Find("Name"))
            nameText = transform.Find("Name").GetComponent<TMP_Text>();
        if (transform.Find("HealthText"))
            healthText = transform.Find("HealthText").GetComponent<TMP_Text>();
        if (transform.Find("HealthBarSlider"))
            healthSlider = transform.Find("HealthBarSlider").GetComponent<Slider>();
        if (transform.Find("ElementIcon"))
            elementIcon = transform.Find("ElementIcon").GetComponent<Image>();
    }

    private void Start()
    {
        if (character != null)
        {
            CombatUIManager.Instance.teamInfoUIs.Add(this);
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
        if (elementIcon)
            elementIcon.sprite = character.entity.element.elementImage;
    }
    public void UpdateHealthUI()
    {
        healthSlider.maxValue = character.trueStats.maxHealth;  //Update Max HP
        healthSlider.value = character.trueStats.health;    //Set current HP
        if (healthText)
            healthText.text = Mathf.RoundToInt(character.trueStats.health).ToString();
    }
}
