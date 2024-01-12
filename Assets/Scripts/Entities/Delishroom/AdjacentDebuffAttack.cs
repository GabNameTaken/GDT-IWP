using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Adjacent Debuff Attack")]
public class AdjacentDebuffAttack : Skill
{
    [SerializeField] string animationStr;
    [SerializeField] List<StatusEffectData> debuffs;
    public float statusEffectChance = 0.50f;
    public int debuffTurns = 2;
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
            CameraManager.Instance.MoveCamera(MapManager.Instance.battleground, CAMERA_POSITIONS.PLAYER_TEAM_FRONT, 0f);
            base.Use(attacker, attackeeList);
        });
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        foreach (StatusEffectData debuff in debuffs)
        {
            foreach (EntityBase attackee in attackeeList)
            {
                if (RunProbability(statusEffectChance))
                    attackee.AddStatusEffect(InitStatusEffect(attacker, attackee, debuffTurns, debuff));
            }
        }
        attacker.excessTurnMeter += excessTurnMeter;
    }
}
