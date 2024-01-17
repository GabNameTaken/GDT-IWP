using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Gather Up")]
public class GatherUp : Skill
{
    [SerializeField] StatusEffectData statusEffectData;
    [SerializeField] int buffDuration;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
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
                attackee.AddStatusEffect(InitStatusEffect(attacker, attackee, buffDuration, statusEffectData));
            }

        attacker.PostSkill();
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        return 0;
    }
}
