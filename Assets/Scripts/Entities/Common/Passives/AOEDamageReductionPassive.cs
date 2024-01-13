using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/AOE DMG Reducation")]
public class AOEDamageReductionPassive : Skill
{
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityTakeDamageEvent += Activate;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityTakeDamageEvent -= Activate;
    }

    public void Activate(EntityBase entityBase)
    {
        if (!entityBase.ContainsSkill(this))
            return;

        entityBase.damageTaken *= multiplier;
    }
}
