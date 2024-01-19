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

    [SerializeField] bool stackable;

    public void Activate()
    {
        foreach (Condition condition in conditions) condition?.Init(stackable);
        OnActivate();
    }

    public void OnActivate()
    {
        foreach (LastingEffect lastingEffect in lastingEffects) lastingEffect.RegisterEffect();
    }

    public void Deactivate()
    {
        foreach (Condition condition in conditions) condition?.Cleanup(stackable);
        OnDeactivate();
    }

    public void OnDeactivate()
    {
        foreach (LastingEffect lastingEffect in lastingEffects) lastingEffect.UnregisterEffect();
    }
}


