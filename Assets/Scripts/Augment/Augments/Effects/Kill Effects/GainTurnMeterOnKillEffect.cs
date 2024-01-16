using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kill Effect", menuName = "Augment/Effect/Kill Effect/Gain TurnMeter On Kill Effect")]
public class GainTurnMeterOnKillEffect : KillEffect
{
    [SerializeField] float turnMeterIncrease;
    public override void RegisterEffect(EntityBase deadEntity)
    {
        CombatManager.Instance.GetEntityTakingTurn().excessTurnMeter += turnMeterIncrease;
    }
}
