using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shatter")]
public class Shatter : Skill
{
    [SerializeField] List<StatusEffectData> statusEffectDatas;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("Shatter");
        CameraManager.Instance.MoveCamera(MapManager.Instance.currentMap.transform.Find("CombatSetup").gameObject, CAMERA_POSITIONS.PLAYER_TEAM_BACK, 0.8f);
        base.Use(attacker, attackeeList);
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        bool crit = false;
        if (IsCriticalHit(attacker.trueStats.critRate))
        {
            damage = (int)Mathf.Round(((attacker.trueStats.attack * 0.9f + attacker.trueStats.maxHealth * 0.04f) * (multiplier + additionalScalings) - attackee.trueStats.defense) * (attacker.trueStats.critDMG / 100));
            crit = true;
        }
        else
            damage = (int)Mathf.Round((attacker.trueStats.attack * 0.9f + attacker.trueStats.maxHealth * 0.04f) * (multiplier + additionalScalings) - attackee.trueStats.defense);

        if (damage <= 0)
            damage = 10;

        CombatUIManager.Instance.ShowDMGNumbers(damage, crit);
        return damage;
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);

        SkillParticle particle = Instantiate(skillParticle, attacker.model.transform);
        particle.Play();

        foreach (PlayableCharacter playable in CombatManager.Instance.PlayerParty)
        {
            if (!playable.IsDead)
            {
                foreach (StatusEffectData data in statusEffectDatas)
                {
                    playable.AddStatusEffect(new StatusEffect(attacker, playable, 2, data));
                }
            }
        }

        foreach (EntityBase attackee in attackeeList)
            attackee.TakeDamage(CalculateDamage(attacker, attackee), crit, attacker.entity.element);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.6f);

        attacker.TakeDamage(-attacker.trueStats.maxHealth * 0.04f, false, null, false);

        attacker.PostSkill();
    }
}
