using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Light Slash")]
public class LightSlash : Skill
{
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1);
        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);

        attacker.animator.Play("Move");
        Tween moveTween = attacker.transform.DOMove(targetPos, 1);
        moveTween.OnComplete(() =>
        {
            CombatManager.Instance.turnCharge.AddEther(1);
            attacker.animator.Play("LightSlashAttack");
            base.Use(attacker, attackeeList);
        });
    }

    protected override void ApplyStatusEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attackeeList[0].TurnMeter -= 20;
    }
}
