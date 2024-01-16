using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitCondition", menuName = "Augment/Conditions/Hit Condition")]
public class HitCondition : EventCondition<EntityBase>
{
    public override void AttachEvent()
    {
        CombatManager.Instance.EntityTakeDamageEvent += OnGameEvent;
    }

    public override void DetachEvent()
    {
        CombatManager.Instance.EntityTakeDamageEvent -= OnGameEvent;
    }

    protected override void OnGameEvent(EntityBase hitEntity)
    {
        if (!Check()) return;
        Effect.RegisterEffect(hitEntity);
    }
}