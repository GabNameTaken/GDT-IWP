using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Night Night")]
public class NightNight : Skill
{
    [SerializeField] StatusEffectData statusEffectData;
    public float debuffChance = 0.3f;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("NightNightAttack");

        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        base.Use(attacker, attackeeList);
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        if(RunProbability(debuffChance))
            attackeeList[0].AddStatusEffect(new StatusEffect(attacker, attackeeList[0], 1, statusEffectData));
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        if (IsCriticalHit(attacker.trueStats.critRate))
            damage = (int)Mathf.Round((attacker.trueStats.maxHealth  * (multiplier + additionalScalings) - attackee.trueStats.defense) * (attacker.trueStats.critDMG / 100));
        else
            damage = (int)Mathf.Round(attacker.trueStats.maxHealth * (multiplier + additionalScalings) - attackee.trueStats.defense);

        return damage;
    }
}
