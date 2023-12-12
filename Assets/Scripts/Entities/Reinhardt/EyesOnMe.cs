using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/EyesOnMe")]
public class EyesOnMe : Skill
{
    [SerializeField] StatusEffectData provokeDebuffData;

    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("EyesOnMe");

        foreach (Enemy enemy in CombatManager.Instance.EnemyParty)
            enemy.AddStatusEffect(new StatusEffect(attacker, enemy, 1, provokeDebuffData));

        base.Use(attacker, attackee);
    }
}
