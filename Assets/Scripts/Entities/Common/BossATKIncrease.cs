using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/Boss ATK Increase")]
public class BossATKIncrease : Skill
{
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityEndTurnEvent += Activate;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityEndTurnEvent -= Activate;
    }

    public void Activate(EntityBase entityBase)
    {
        if (!entityBase.ContainsSkill(this))
            return;

        entityBase.trueStats.attack += entityBase.entity.baseStats.Stats.attack * multiplier;
    }
}
