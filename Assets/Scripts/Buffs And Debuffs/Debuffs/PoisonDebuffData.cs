using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Poison")]
public class PoisonDebuffData : DebuffData
{
    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.TakeDamage(dest.trueStats.maxHealth * (multiplier / 100));
        Debug.Log(dest.trueStats.health + " - " + dest.trueStats.maxHealth * (multiplier / 100));
    }
}
