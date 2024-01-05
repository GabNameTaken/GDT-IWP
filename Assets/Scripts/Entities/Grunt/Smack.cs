using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Smack")]
public class Smack : Skill
{
    [SerializeField] List<StatusEffectData> debuffs;
    public float statusEffectChance = 1.00f;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);
        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 1);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("SmackAttack");
            CombatManager.Instance.turnCharge.AddEther(1);
            base.Use(attacker, attackeeList);
            foreach (StatusEffectData data in debuffs)
            {
                if (RunProbability(statusEffectChance))
                    attackeeList[0].AddStatusEffect(InitStatusEffect(attacker, attackeeList[0], 2, data));
            }
        });
    }
}
