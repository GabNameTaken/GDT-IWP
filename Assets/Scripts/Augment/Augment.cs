using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augment/Augment")]
public class Augment : ScriptableObject
{
    [SerializeField] private string label, description;
    public string Label => label; public string Description => description;


    [SerializeField] private AugmentRarity.RARITY rarity;
    public AugmentRarity AugmentRarity => AugmentManager.Instance?.GetAugmentRarity(rarity);

    [SerializeField] List<Condition> conditions;
    [SerializeField] List<LastingEffect> lastingEffects;

    public void Activate()
    {
        if (conditions.Count < 1) return;

        foreach (Condition condition in conditions) condition?.Init();
        OnActivate();
    }

    public void OnActivate()
    {
        foreach (LastingEffect lastingEffect in lastingEffects) lastingEffect.RegisterEffect();
    }

    public void Deactivate()
    {
        if (conditions.Count < 1) return;

        foreach (Condition condition in conditions) condition?.Cleanup();
        OnDeactivate();
    }

    public void OnDeactivate()
    {
        foreach (LastingEffect lastingEffect in lastingEffects) lastingEffect.UnregisterEffect();
    }
}


