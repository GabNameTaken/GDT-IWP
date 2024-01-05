using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/HeavySlash")]
public class HeavySlash : Skill
{
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);
        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 0.5f);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("HeavySlashAttack");
            base.Use(attacker, attackeeList);
        });
    }
}
