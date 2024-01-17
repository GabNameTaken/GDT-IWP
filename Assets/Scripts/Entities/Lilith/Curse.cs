using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Curse")]
public class Curse : Skill
{
    [SerializeField] PoisonDebuffData poisonDebuffData;
    public float poisonChance = 0.75f;

    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("CurseAttack");
        base.Use(attacker, attackeeList);
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        foreach (EntityBase attackee in attackeeList)
            if (!attackee.IsDead)
            {
                SkillParticle particle = Instantiate(skillParticle, attackee.transform);
                particle.Play();
                attackee.TakeDamage(CalculateDamage(attacker, attackee), crit, attacker.entity.element);
                if (RunProbability(poisonChance))
                    attackee.AddStatusEffect(InitStatusEffect(attacker, attackee, 2, poisonDebuffData));
            }

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.7f);

        attacker.PostSkill();
    }
} 
