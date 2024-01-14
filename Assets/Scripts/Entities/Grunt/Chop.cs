using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Chop")]
public class Chop : Skill
{
    [SerializeField] StatusEffectData statusEffectData;
    public float statusEffectChance = 0.50f;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.transform.DORotateQuaternion(GetQuaternionRotationToTarget(attacker.transform.position, attackeeList[0].transform.position), 0.5f);
        Vector3 targetPos = GetFrontPos(attacker.transform.position, attackeeList[0].transform.position, 1.5f);

        Tween moveTween = attacker.transform.DOJump(targetPos, 0.5f, 1, 1);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("ChopAttack");
            CombatManager.Instance.turnCharge.AddEther(1);
            base.Use(attacker, attackeeList);
            if (RunProbability(statusEffectChance))
                attackeeList[0].AddStatusEffect(InitStatusEffect(attacker, attackeeList[0], 2, statusEffectData));
        });
    }
}
