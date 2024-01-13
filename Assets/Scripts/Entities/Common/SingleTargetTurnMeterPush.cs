using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName ="Skills/Single Target TurnMeter Push")]
public class SingleTargetTurnMeterPush : Skill
{
    [SerializeField] string animationStr;
    public float amount;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play(animationStr);
        base.Use(attacker, attackeeList);
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attackeeList[0].TurnMeter += amount;
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        return 0;
    }
}
