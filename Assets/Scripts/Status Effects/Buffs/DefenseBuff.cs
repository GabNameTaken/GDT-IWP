using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Defense")]
public class DefenseBuff : StatusEffectData
{
    [SerializeField] float defenseBuffMultiplier;

    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.defense *= (defenseBuffMultiplier / 100f);
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.defense /= (defenseBuffMultiplier / 100f);
    }
}
