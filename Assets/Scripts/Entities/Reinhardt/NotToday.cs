using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotToday : Skill
{
    int reviveNum = 1;
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
        if (reviveNum > 0 && entityBase.isDead)
        {
            entityBase.isDead = false;
            entityBase.TakeDamage(entityBase.trueStats.maxHealth * 0.25f, null);
            entityBase.animator.Play("Idle");
        }
    }
}
