using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    Slider healthSlider;
    TMP_Text healthText, nameText;

    public EntityBase character;

    private void Awake()
    {
        nameText = transform.Find("Name").GetComponent<TMP_Text>();
        healthText = transform.Find("HealthText").GetComponent<TMP_Text>();
        healthSlider = transform.Find("HealthBarSlider").GetComponent<Slider>();
    }

    public void SetUpUI()
    {
        nameText.text = character.entity.entityName;
        healthSlider.maxValue = character.trueStats.maxHealth;  //Update Max HP
        healthSlider.value = character.trueStats.health;    //Set current HP
        healthText.text = Mathf.RoundToInt(character.trueStats.health).ToString();
    }
    public void UpdateHealthUI()
    {
        healthSlider.maxValue = character.trueStats.maxHealth;  //Update Max HP
        healthSlider.value = character.trueStats.health;    //Set current HP
        healthText.text = Mathf.RoundToInt(character.trueStats.health).ToString();
    }
}
