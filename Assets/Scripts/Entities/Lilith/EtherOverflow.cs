using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/EtherOverflow")]
public class EtherOverflow : Skill
{
    [SerializeField] List<StatusEffectData> statusEffectDatas;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("EtherOverflow");

        List<EntityBase> alive = new();
        foreach (PlayableCharacter playable in CombatManager.Instance.PlayerParty)
        {
            if (!playable.isDead)
                alive.Add(playable);
        }
        base.Use(attacker, alive);
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        base.SkillAnimationCoroutine(attacker, attackeeList);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.7f);

        SkillParticle particle = Instantiate(skillParticle, attacker.transform);
        particle.Play();

        foreach (EntityBase attackee in attackeeList)
        {
            foreach (StatusEffectData data in statusEffectDatas)
            {
                attackee.AddStatusEffect(new StatusEffect(attacker, attackee, 2, data));
            }
        }

        yield return base.SkillAnimationCoroutine(attacker, attackeeList);
    }
}
