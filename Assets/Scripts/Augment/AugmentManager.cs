using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
using System.Linq;

public class AugmentManager : Singleton<AugmentManager>
{
    List<Augment> ownedAugments = new List<Augment>(), activeAugments = new List<Augment>();

    AugmentPool augmentPool;

    protected override void Awake()
    {
        base.Awake();

        augmentPool = GetComponent<AugmentPool>();
    }

    void ActivateAugments()
    {
        ownedAugments.ForEach(augment =>
        {
            if (!activeAugments.Contains(augment))
            {
                augment.Activate();
                activeAugments.Add(augment);
            }
        });
    }

    void DeactivateAugments()
    {
        activeAugments.ForEach(activeAugment => activeAugment.Deactivate());
        activeAugments.Clear();
    }

    void RefreshAugments() // Deactivate active augments and activate augment effects in ownedAugments
    {
        DeactivateAugments(); ActivateAugments();
    }

    public AugmentRarity GetAugmentRarity(AugmentRarity.RARITY rarity)
    {
        if (!augmentPool)
        {
            Debug.Log("AugmentPool is null."); return null; 
        }
        return augmentPool.augmentRarityDictionary[rarity];
    }
}
