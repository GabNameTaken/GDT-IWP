using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AugmentPool : MonoBehaviour
{
    [SerializeField] List<Augment> augmentPool = new List<Augment>();


    [SerializeField] List<AugmentRarityPair> augmentRarityPairList = new List<AugmentRarityPair>();
    public Dictionary<AugmentRarity.RARITY, AugmentRarity> augmentRarityDictionary => augmentRarityPairList.ToDictionary(pair => pair.rarity, pair => pair.augmentRarity);


    [SerializeField] List<AugmentRarityChancePair> augmentRarityChancePairList = new List<AugmentRarityChancePair>();
    public Dictionary<AugmentRarity.RARITY, float> augmentRarityChanceDictionary => augmentRarityChancePairList.ToDictionary(pair => pair.rarity, pair => pair.chance);

    public Augment GenerateRandomAugment(List<Augment> augmentExceptions)
    {
        Augment randomAugment = null;

        float randomFloat = Random.Range(0f, augmentRarityChanceDictionary.Values.Sum()); // Sum up all the chances

        float totalChance = 0f;
        foreach (AugmentRarity.RARITY rarity in augmentRarityChanceDictionary.Keys)
        {
            totalChance += augmentRarityChanceDictionary[rarity]; // Add chance
            if (randomFloat < totalChance) // If chance hit
            {
                List<Augment> augmentsOfRarity = new List<Augment>(augmentPool); // Make new list with only augments of rarity
                augmentsOfRarity.RemoveAll(augment => augment.AugmentRarity.Rarity != rarity); // Remove augments that are not of rarity
                while (!randomAugment && augmentExceptions.Contains(randomAugment)) // If augment not acceptable, reroll
                    randomAugment = augmentsOfRarity[Random.Range(0, augmentsOfRarity.Count)]; // Randomise augment
            }
        }
        return randomAugment;
    }
}

[System.Serializable]
public class AugmentRarityPair
{
    public AugmentRarity.RARITY rarity;
    public AugmentRarity augmentRarity;
}

[System.Serializable]
public class AugmentRarityChancePair
{
    public AugmentRarity.RARITY rarity;
    public float chance;
}
