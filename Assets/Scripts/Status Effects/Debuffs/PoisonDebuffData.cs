using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Poison")]
public class PoisonDebuffData : StatusEffectData
{
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.TakeDamage(dest.trueStats.maxHealth * (multiplier / 100), null);
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
    }
}
