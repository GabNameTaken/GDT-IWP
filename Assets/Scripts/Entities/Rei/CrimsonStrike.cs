using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/CrimsonStrike")]
public class CrimsonStrike : Skill
{
    [SerializeField] float speedScaling;

    [SerializeField] StatusEffectData attackBuff;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        CameraManager.Instance.MoveCamera(attackee.gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackee.transform.position, 1);
        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackee.transform.position), 0.5f);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 0.5f);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("CrimsonStrikeAttack");
            additionalScalings = attacker.trueStats.speed * speedScaling;
            base.Use(attacker, attackee);
            attacker.AddStatusEffect(InitStatusEffect(attacker, attacker, 2, attackBuff));
            Debug.Log(attacker.trueStats.attack);
        });
        
    }
}
