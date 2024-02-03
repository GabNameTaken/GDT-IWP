using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AugmentRarity", menuName = "Augment/Augment Rarity")]
public class AugmentRarity : ScriptableObject
{
    public enum RARITY
    {
        COMMON,
        RARE,
        EPIC
    }

    [SerializeField] string label; public string Label => label; 
    [SerializeField] RARITY rarity; public RARITY Rarity => rarity;
    [SerializeField] Sprite cardBackground; public Sprite CardBackground => cardBackground;

    [SerializeField] Color cardBackgroundColor; public Color backgroundColor => cardBackgroundColor;
}
