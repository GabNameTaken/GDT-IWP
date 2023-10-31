using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Cut")]
public class Cut : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("CutAttack");
        attackee.TakeDamage(CalculateDamage(attacker, attackee));
    }
}
