using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Excalibur!")]
public class Excalibur : Skill
{
    [SerializeField] List<StatusEffectData> buffs;

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
            attacker.animator.Play("ExcaliburAttack");
            base.Use(attacker, attackeeList);
        });
    }

    protected override void ApplyStatusEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        foreach (StatusEffectData data in buffs)
        {
            attacker.AddStatusEffect(InitStatusEffect(attacker, attacker, 3, data));
        }
    }
}
