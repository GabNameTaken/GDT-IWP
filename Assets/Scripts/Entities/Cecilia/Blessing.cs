using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Blessing")]
public class Blessing : Skill
{
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("Blessing");
        base.Use(attacker, attackeeList);
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        damage = Mathf.RoundToInt(attacker.trueStats.health * multiplier);
        
        return -damage;
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        base.SkillAnimationCoroutine(attacker, attackeeList);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        SkillParticle particle = Instantiate(skillParticle, attacker.transform);
        particle.Play();

        yield return base.SkillAnimationCoroutine(attacker, attackeeList);
    }
}
