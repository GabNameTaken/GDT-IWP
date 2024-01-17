using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] StatusIcon iconPrefab;

    public void OnAddStatus(StatusEffect statusEffect, int duration)
    {
        StatusIcon icon = Instantiate(iconPrefab, transform);
        icon.GetComponent<Image>().sprite = statusEffect.StatusEffectData.icon;
        statusEffect.icon = icon;
        icon.GetComponent<StatusIcon>().InitIcon(statusEffect);

        TMP_Text durationText = icon.transform.GetChild(0).GetComponent<TMP_Text>();
        durationText.text = duration.ToString();
    }

    public void UpdateStatus(StatusIcon icon, int duration)
    {
        icon.durationText.text = duration.ToString();

        if (duration <= 0)
            StartCoroutine(RemoveCoroutine(icon));
    }

    IEnumerator RemoveCoroutine(StatusIcon icon)
    {
        yield return new WaitForSeconds(3f);

        RemoveStatus(icon);
    }
    public void RemoveStatus(StatusIcon icon)
    {
        Destroy(icon.gameObject);
    }
}
