using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [SerializeField] TMP_Text skillNameText;
    [SerializeField] TMP_Text skillDescText;
    [SerializeField] TMP_Text cooldownText;
    [SerializeField] TMP_Text skillCostText;

    public void DisplayUI(Skill skill)
    {
        gameObject.SetActive(false);
        skillNameText.text = skill.skillName;
        skillDescText.text = skill.skillDesc;
        cooldownText.text = "Cooldown: " + skill.cooldown.ToString();
        if (skill.skillCost >= 0)
            skillCostText.text = "Cost: " + skill.skillCost;
        else
            skillCostText.text = "Cost: " + "+" + -skill.skillCost;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(+Screen.width, 0);
        SlideIn();
    }

    float slideDuration = 0.5f;
    void SlideIn()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Use DoTween to slide the image in
        rectTransform.DOAnchorPosX(0, slideDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(UpdateType.Normal, true); // Set timeScaleIndependence to true
    }
}
