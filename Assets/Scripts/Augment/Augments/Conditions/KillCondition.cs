using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillCondition", menuName = "Augment/Conditions/Kill Condition")]
public class KillCondition : EventCondition<EntityBase>
{
    public override void AttachEvent()
    {
        CombatManager.Instance.EntityDeadEvent += OnGameEvent;
    }

    public override void DetachEvent()
    {
        CombatManager.Instance.EntityDeadEvent -= OnGameEvent;
    }

    protected override void OnGameEvent(EntityBase deadEntity)
    {
        if (!Check()) return;
        Effect.RegisterEffect(deadEntity);
    }
}
