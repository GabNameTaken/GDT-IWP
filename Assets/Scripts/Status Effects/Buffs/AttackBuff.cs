using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Attack")]
public class AttackBuff : StatusEffectData
{
    [SerializeField] float attackBuffPercentage;

    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack *= (attackBuffPercentage / 100f);
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack /= (attackBuffPercentage / 100f);
    }
}
