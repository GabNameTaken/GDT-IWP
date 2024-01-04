using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/Grunts' Patience")]
public class GruntPatience : Skill
{
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityStartTurnEvent += Activate;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityStartTurnEvent -= Activate;
    }

    public void Activate(EntityBase entityBase)
    {
        if (!entityBase.GetComponent<Enemy>() || entityBase.ContainsSkill(this))
            return;
        CombatManager.Instance.entitiesOnField.ForEach(entity =>
        {
            if (entity.ContainsSkill(this))
            {
                entity.TurnMeter += 20f;
                CombatManager.Instance.UpdateTurnOrderUI();
            }
        });
    }
}
