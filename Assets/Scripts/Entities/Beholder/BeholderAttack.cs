using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu (menuName = "Skills/Beholder Attack")]
public class BeholderAttack : Skill
{
    [SerializeField] string animationStr;

    [Header("Debuffs")]
    [SerializeField] List<StatusEffectData> debuffs;
    [SerializeField] float debuffChance;
    [SerializeField] int debuffTurns;

    [Header("Self Buff")]
    [SerializeField] List<StatusEffectData> buffs;
    [SerializeField] int buffTurns;

    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.LOW_BACK, 1f);

        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1.5f);
        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);

        attacker.animator.Play("Move");
        Tween moveTween = attacker.transform.DOMove(targetPos, 1.8f);
        moveTween.OnComplete(() =>
        {
            CombatManager.Instance.turnCharge.AddEther(1);
            attacker.animator.Play(animationStr);
            CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0f);
            base.Use(attacker, attackeeList);
        });
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        foreach (EntityBase attackee in attackeeList)
        {
            foreach (StatusEffectData data in debuffs)
            {
                if (RunProbability(debuffChance))
                    attackee.AddStatusEffect(InitStatusEffect(attacker, attackee, debuffTurns, data));
            }
        }
        foreach (StatusEffectData data in buffs)
            attacker.AddStatusEffect(InitStatusEffect(attacker, attacker, buffTurns, data));
    }
}
