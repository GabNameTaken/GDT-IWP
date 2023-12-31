using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Gather Up")]
public class GatherUp : Skill
{
    [SerializeField] StatusEffectData statusEffectData;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        attacker.animator.Play("ChopAttack");
        base.Use(attacker, attackeeList);
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.7f);

        foreach (EntityBase attackee in attackeeList)
            if (!attackee.IsDead)
            {
                //SkillParticle particle = Instantiate(skillParticle, attackee.transform);
                //particle.Play();
                attackee.AddStatusEffect(InitStatusEffect(attacker, attackee, 3, statusEffectData));
            }

        attacker.PostSkill();
    }
}
