using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hit Effect", menuName = "Augment/Effect/Hit Effect/Apply Debuff Effect")]
public class ApplyDebuffEffect : HitEffect
{
    [SerializeField] StatusEffectData statusEffectData;
    [SerializeField] int duration;

    public override void RegisterEffect(EntityBase hitEntity)
    {
        hitEntity.AddStatusEffect(new StatusEffect(CombatManager.Instance.GetEntityTakingTurn(), hitEntity, duration, statusEffectData));
    }
}
