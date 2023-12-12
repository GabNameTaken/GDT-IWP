using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Night Night")]
public class NightNight : Skill
{
    [SerializeField] StatusEffectData statusEffectData;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("NightNightAttack");

        CameraManager.Instance.MoveCamera(attackee.gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        base.Use(attacker, attackee);
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        foreach (EntityBase attackee in attackeeList)
            if (!attackee.isDead)
            {
                attackee.TakeDamage(CalculateDamage(attacker, attackee), attacker.entity.element);
                attackee.AddStatusEffect(new StatusEffect(attacker, attackee, 1, statusEffectData));
            }

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.7f);

        attacker.PostSkill();
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        if (IsCriticalHit(attacker.trueStats.critRate))
            damage = (int)Mathf.Round((attacker.trueStats.maxHealth  * (multiplier + additionalScalings) - attackee.trueStats.defense / 2) * (attacker.trueStats.critDMG / 100));
        else
            damage = (int)Mathf.Round(attacker.trueStats.maxHealth * (multiplier + additionalScalings) - attackee.trueStats.defense / 2);
        return damage;
    }
}
