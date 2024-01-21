using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/Rizz")]
public class Rizz : Skill
{
    public override void OnBattleStart()
    {
        CombatManager.Instance.EntityTakeDamageEvent += Activate;
        CombatManager.Instance.EntityDeadEvent += Deactivate;
    }

    public override void OnBattleEnd()
    {
        CombatManager.Instance.EntityTakeDamageEvent -= Activate;
        CombatManager.Instance.EntityDeadEvent -= Deactivate;
    }

    public void Activate(EntityBase entityBase)
    {
        if (entityBase.ContainsSkill(this) || entityBase.GetComponent<PlayableCharacter>())
            return;
        entityBase.damageTaken *= multiplier;
    }

    void Deactivate(EntityBase entityBase)
    {
        if (entityBase.ContainsSkill(this))
            CombatManager.Instance.EntityTakeDamageEvent -= Activate;
        else
            return;
    }
}
