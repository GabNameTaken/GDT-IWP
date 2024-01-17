using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "End Turn Effect", menuName = "Augment/Effect/End Turn Effect/Increase Team TurnMeter Effect")]
public class TeamTurnMeterIncreaseEffect : TurnEndEffect
{
    [SerializeField] float turnMeterIncrease;
    public override void RegisterEffect(EntityBase turnEndedEntity)
    {
        foreach (PlayableCharacter player in CombatManager.Instance.PlayerParty)
        {
            if (player.IsDead)
                continue;
            player.TurnMeter += turnMeterIncrease;
        }
    }
}
