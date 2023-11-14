using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Night Night")]
public class NightNight : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("NightNightAttack");
        base.Use(attacker, attackee);
    }
}
