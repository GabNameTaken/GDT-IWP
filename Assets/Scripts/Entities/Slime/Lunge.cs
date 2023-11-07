using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Skills/Lunge")]
public class Lunge : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("LungeAttack");
        base.Use(attacker, attackee);
    }
}
