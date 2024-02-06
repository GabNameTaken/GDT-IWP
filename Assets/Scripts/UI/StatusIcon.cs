using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour
{
    public TMP_Text durationText;
    [SerializeField] GameObject inspectDisplayPrefab;

    GameObject inspectDisplay;
    Button button;
    StatusEffect statusEffect;
    public void InitIcon(StatusEffect statusEffect)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { ToggleInspect(); });
        this.statusEffect = statusEffect;
    }

    void ToggleInspect()
    {
        if (inspectDisplay)
            Destroy(inspectDisplay);
        else
        {
            InitInspect();
        }
    }

    void InitInspect()
    {
        Transform combatUI = GameObject.Find("Canvas").transform.GetChild(0).transform;
        inspectDisplay = Instantiate(inspectDisplayPrefab, combatUI);

        Button button = inspectDisplay.transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(delegate { 
            Destroy(inspectDisplay); 
            inspectDisplay = null; });

        Transform background = inspectDisplay.transform.GetChild(1).transform;
        background.GetChild(0).GetComponent<TMP_Text>().text = statusEffect.StatusEffectData.statusEffectName;
        background.GetChild(1).GetComponent<TMP_Text>().text = statusEffect.StatusEffectData.desc;
        background.GetChild(2).GetComponent<Image>().sprite = statusEffect.StatusEffectData.icon;
        background.GetChild(3).GetComponent<TMP_Text>().text = statusEffect.remainingDuration + " Turns";
    }

    private void FixedUpdate()
    {
        if (statusEffect.remainingDuration == 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (inspectDisplay)
            Destroy(inspectDisplay);
    }
}
