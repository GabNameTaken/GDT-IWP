using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/Not Today")]
public class NotToday : Skill
{
    int reviveNum = 1;
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityDeadEvent += Activate;
        reviveNum = 1;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityDeadEvent -= Activate;
    }

    public void Activate(EntityBase entityBase)
    {
        if (reviveNum <= 0)
            return;
        CombatManager.Instance.entitiesOnField.ForEach(entity =>
        {
            if (entity.ContainsSkill(this) && entity.IsDead)
            {
                entityBase.IsDead = false;
                entityBase.TakeDamage(-(entityBase.trueStats.maxHealth * 0.25f), false, null, false);
                entityBase.animator.Play("Idle");
                reviveNum--;
            }
        });
    }
}
