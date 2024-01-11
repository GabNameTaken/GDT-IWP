using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntityInfoUI : MonoBehaviour
{
    public EntityBase character;
    
    [Header("Manual SetUp")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthText, nameText;
    [SerializeField] Image elementIcon;
    [SerializeField] Image classIcon;

    [SerializeField] StatusEffectUI _statusEffectUI;
    public StatusEffectUI statusEffectUI => _statusEffectUI;

    private void Awake()
    {
        if (!nameText)
        {
            if (transform.Find("Name"))
                nameText = transform.Find("Name").GetComponent<TMP_Text>();
        }
        if (!healthText)
        {
            if (transform.Find("HealthText"))
                healthText = transform.Find("HealthText").GetComponent<TMP_Text>();
        }
        if (!healthSlider)
        {
            if (transform.Find("HealthBarSlider"))
                healthSlider = transform.Find("HealthBarSlider").GetComponent<Slider>();
        }
        if (!elementIcon)
        {
            if (transform.Find("ElementIcon"))
                elementIcon = transform.Find("ElementIcon").GetComponent<Image>();
        }
    }

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
}
