using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/CrimsonStrike")]
public class CrimsonStrike : Skill
{
    [SerializeField] float speedScaling;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        Tween moveTween = attacker.transform.DOJump(attackee.transform.position, 0.5f, 1, 0.5f);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("CrimsonStrikeAttack");
            additionalScalings = attacker.trueStats.speed * speedScaling;
            base.Use(attacker, attackee);
        });
        
    }
}
