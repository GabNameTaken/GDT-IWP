using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Purification")]
public class Purification : Skill
{
    [SerializeField] List<StatusEffectData> statusEffectDatas;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("Purification");
        List<EntityBase> alive = new();
        foreach (PlayableCharacter playable in CombatManager.Instance.PlayerParty)
        {
            if (!playable.isDead)
                alive.Add(playable);
        }
        base.Use(attacker, alive);
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        damage = Mathf.RoundToInt(attacker.trueStats.health * multiplier);
        return -damage;
    }
    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);

        SkillParticle particle = Instantiate(skillParticle, attacker.transform);
        particle.Play();

        foreach (EntityBase attackee in attackeeList)
        {
            attackee.TakeDamage(CalculateDamage(attacker, attackee), attacker.entity.element);

            foreach (StatusEffectData data in statusEffectDatas)
            {
                attackee.AddStatusEffect(new StatusEffect(attacker, attackee, 1, data));
            }

            List<StatusEffect> debuffs = attackee.statusEffectList.Where((a) => a.StatusEffectData.type == STATUS_EFFECT_TYPE.DEBUFF).ToList();
            if (debuffs.Count > 0)
                attackee.RemoveStatusEffect(debuffs.First());
        }

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.7f);

        attacker.PostSkill();
    }
}
