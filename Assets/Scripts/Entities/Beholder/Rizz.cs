using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkills/Rizz")]
public class Rizz : Skill
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
        if (entityBase.ContainsSkill(this) || entityBase.GetComponent<PlayableCharacter>())
            return;

        entityBase.damageTaken *= multiplier;
    }
}
