using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/SilentAssassination")]
public class SilentAssassination : Skill
{
    [SerializeField] float speedScaling;

    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        SkillParticle particle = Instantiate(skillParticle, attacker.transform);
        particle.ManualPlay(0.25f);

        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, -2);
        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);

        Tween moveTween = attacker.transform.DOMove(targetPos, 0.2f, true).SetEase(Ease.Flash);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("SilentAssassinationAttack");
            additionalScalings = attacker.trueStats.speed * speedScaling;
            base.Use(attacker, attackeeList);
        });
    }
}