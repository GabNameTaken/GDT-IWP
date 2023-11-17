using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/EyesOnMe")]
public class EyesOnMe : Skill
{
    [SerializeField] DebuffData provokeDebuffData;

    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("EyesOnMe");
        stayOnAnimation = true;

        foreach (Enemy enemy in CombatManager.Instance.enemyParty)
            enemy.debuffList.Add(InitDebuff(attacker, enemy, 1, provokeDebuffData));

        base.Use(attacker, attackee);
    }
}
