using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Speed")]
public class SpeedBuff : StatusEffectData
{
    [SerializeField] float speedBuffMultiplier;

    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.speed *= Mathf.RoundToInt(speedBuffMultiplier / 100f);
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.speed /= Mathf.RoundToInt(speedBuffMultiplier / 100f);
    }
}
