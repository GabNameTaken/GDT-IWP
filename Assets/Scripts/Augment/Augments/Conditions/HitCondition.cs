using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (CombatManager.Instance.PlayerParty.Contains(hitEntity))
            return;
        Effect.RegisterEffect(hitEntity);
    }
}