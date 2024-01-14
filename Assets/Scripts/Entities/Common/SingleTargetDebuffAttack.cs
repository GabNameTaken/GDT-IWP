using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Single Target Debuff Attack")]
public class SingleTargetDebuffAttack : Skill
{
    [SerializeField] string animationStr;
    [SerializeField] List<StatusEffectData> debuffs;
    public float statusEffectChance = 0.50f;
    public int debuffTurns = 2;
    public float excessTurnMeter = 0;
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
        foreach (StatusEffectData debuff in debuffs)
        {
            if (RunProbability(statusEffectChance))
                attackeeList[0].AddStatusEffect(InitStatusEffect(attacker, attackeeList[0], debuffTurns, debuff));
        }
        attacker.excessTurnMeter += excessTurnMeter;
    }
}
