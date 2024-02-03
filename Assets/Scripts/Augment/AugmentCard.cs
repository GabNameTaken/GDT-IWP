using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AugmentCard : MonoBehaviour
{
    [SerializeField] Image cardBackground;
    [SerializeField] TMP_Text labelText, descriptionText;
    Augment augment;

    private void Awake()
    {
        augment = AugmentManager.Instance.GetRandomAugment();

        // If there is a card background for the rarity, replace the card background
        if (augment.AugmentRarity.CardBackground) cardBackground.sprite = augment.AugmentRarity.CardBackground;
        cardBackground.color = augment.AugmentRarity.backgroundColor;

        labelText.text = augment.Label;
        descriptionText.text = augment.Description;
    }

    void AddAugment()
    {
        AugmentManager.Instance.AddAugment(augment);
    }

    public void OnCardClick()
    {
        AddAugment();
    }
}
