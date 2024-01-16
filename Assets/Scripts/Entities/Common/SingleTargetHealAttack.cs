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

    EntityBase healedEntity = null;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
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
        attacker.excessTurnMeter += excessTurnMeter;
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        foreach (EntityBase attackee in attackeeList)
            if (!attackee.IsDead)
                attackee.TakeDamage(CalculateDamage(attacker, attackee), attacker.entity.element);

        ApplyEffects(attacker, attackeeList);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.8f);

        healedEntity = CombatManager.Instance.EnemyParty
            .Where(enemy => !enemy.IsDead)
            .OrderBy(enemy => enemy.trueStats.health / enemy.trueStats.maxHealth)
            .FirstOrDefault();

        CameraManager.Instance.MoveCamera(healedEntity.gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 1f);

        yield return new WaitForSeconds(1.2f);

        float heal = healedEntity.trueStats.maxHealth * healMultiplier;
        healedEntity.TakeDamage(-heal, null);
        CombatUIManager.Instance.ShowDMGNumbers(heal, false);

        yield return new WaitForSeconds(1.2f);

        attacker.PostSkill();
    }
}
