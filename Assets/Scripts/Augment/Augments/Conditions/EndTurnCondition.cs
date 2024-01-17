using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EndTurnCondition", menuName = "Augment/Conditions/End Turn Condition")]
public class EndTurnCondition : EventCondition<EntityBase>
{
    public override void AttachEvent()
    {
        CombatManager.Instance.EntityEndTurnEvent += OnGameEvent;
    }

    public override void DetachEvent()
    {
        CombatManager.Instance.EntityEndTurnEvent -= OnGameEvent;
    }

    protected override void OnGameEvent(EntityBase turnEndedEntity)
    {
        if (!Check()) return;
        Effect.RegisterEffect(turnEndedEntity);
    }
}
