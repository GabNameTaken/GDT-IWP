using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] GameObject iconPrefab;

    public void OnAddStatus(Debuff debuff, int duration)
    {
        GameObject icon = Instantiate(iconPrefab, transform);
        icon.GetComponent<Image>().sprite = debuff.data.icon;
        debuff.icon = icon;

        TMP_Text durationText = icon.transform.GetChild(0).GetComponent<TMP_Text>();
        durationText.text = duration.ToString();
    }

    public void UpdateStatus(GameObject icon, int duration)
    {
        TMP_Text durationText = icon.transform.GetChild(0).GetComponent<TMP_Text>();
        durationText.text = duration.ToString();

        if (duration <= 0)
            StartCoroutine(RemoveCoroutine(icon));
    }

    IEnumerator RemoveCoroutine(GameObject icon)
    {
        yield return new WaitForSeconds(3f);

        RemoveStatus(icon);
    }
    public void RemoveStatus(GameObject icon)
    {
        Destroy(icon);
    }
}
