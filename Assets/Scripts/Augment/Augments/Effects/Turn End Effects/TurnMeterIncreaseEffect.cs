using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "End Turn Effect", menuName = "Augment/Effect/End Turn Effect/Increase TurnMeter Effect")]
public class TurnMeterIncreaseEffect : TurnEndEffect
{
    [SerializeField] float turnMeterIncrease;
    public override void RegisterEffect(EntityBase turnEndedEntity)
    {
        turnEndedEntity.TurnMeter += turnMeterIncrease;
    }
}
