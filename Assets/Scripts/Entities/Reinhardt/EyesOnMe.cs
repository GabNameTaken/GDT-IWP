using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/EyesOnMe")]
public class EyesOnMe : Skill
{
    [SerializeField] StatusEffectData provokeDebuffData;

    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("EyesOnMe");

        foreach (EntityBase attackee in attackeeList)
            attackee.AddStatusEffect(new StatusEffect(attacker, attackee, 1, provokeDebuffData));

        base.Use(attacker, attackeeList);
    }
}
