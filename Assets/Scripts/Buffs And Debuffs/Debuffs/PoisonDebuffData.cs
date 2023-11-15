using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Poison")]
public class PoisonDebuffData : DebuffData
{
    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.TakeDamage(dest.trueStats.maxHealth * (multiplier / 100));
    }

    protected override float CalculateDMG(EntityBase source, EntityBase dest)
    {
        float damage = dest.trueStats.maxHealth * (multiplier / 100);
        return damage;
    }
}
