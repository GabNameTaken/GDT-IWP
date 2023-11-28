using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Blessing")]
public class Blessing : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("Blessing");
        base.Use(attacker, attackee);
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        damage = Mathf.RoundToInt(attacker.trueStats.health * multiplier);
        
        return -damage;
    }
}
