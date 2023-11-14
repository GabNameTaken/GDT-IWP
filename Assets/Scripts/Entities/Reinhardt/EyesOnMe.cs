using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/EyesOnMe")]
public class EyesOnMe : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("EyesOnMe");
        stayOnAnimation = true;
        base.Use(attacker, attackee);
    }
}
