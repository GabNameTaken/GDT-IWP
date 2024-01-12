using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

[CreateAssetMenu(menuName ="Skills/Single Target Heal Attack")]
public class SingleTargetHealAttack : Skill
{
    [SerializeField] string animationStr;
    [SerializeField] float healMultiplier;
    public float excessTurnMeter = 0;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);
        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1.5f);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 1);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play(animationStr);
            CombatManager.Instance.turnCharge.AddEther(1);
            CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0f);
            base.Use(attacker, attackeeList);
        });
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        EntityBase enemyWithLowestPercentageHP = CombatManager.Instance.EnemyParty
            .OrderBy(enemy => enemy.trueStats.health / enemy.trueStats.maxHealth)
            .FirstOrDefault();
        enemyWithLowestPercentageHP.TakeDamage(-(enemyWithLowestPercentageHP.trueStats.maxHealth * healMultiplier), null);
        attacker.excessTurnMeter += excessTurnMeter;
    }
}
