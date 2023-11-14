using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Curse")]
public class Curse : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("CurseAttack");
        foreach(Enemy enemy in CombatManager.Instance.enemyParty)
        {
            base.Use(attacker, enemy);
        }
    }
}
