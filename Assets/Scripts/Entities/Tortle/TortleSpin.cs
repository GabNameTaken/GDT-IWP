using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/TortleSpin")]
public class TortleSpin : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackee.transform.position), 0.5f);
        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackee.transform.position, 1);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 1);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("TortleSpinAttack");
            CombatManager.Instance.turnCharge.AddEther(1);
            base.Use(attacker, attackee);
        });
    }
}
