using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Opportunity")]
public class Opportunity : Skill
{
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityDeadEvent += Activate;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityDeadEvent -= Activate;
    }

    public void Activate(EntityBase entityBase)
    {
        CombatManager.Instance.entitiesOnField.ForEach(entity =>
        {
            if (entity.isMoving && entity.ContainsSkill(this))
                entity.excessTurnMeter += 50f;
        });
    }
}
