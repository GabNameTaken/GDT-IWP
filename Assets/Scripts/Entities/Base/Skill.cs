using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SKILL_CODE
{
    S1,
    S2,
    S3,
    NONE,
}

public class Skill : ScriptableObject
{
    public enum SKILL_TARGETS
    {
        SINGLE_TARGET,
        ALL,
        ADJACENT,
        NONE,
        TOTAL
    }

    public SKILL_TARGETS targets;

    [SerializeField] protected string _skillName;
    public string skillName => _skillName;

    [SerializeField] protected string _skillDesc;
    public string skillDesc => _skillDesc;

    [SerializeField] protected Sprite _skillIcon;
    public Sprite skillIcon => _skillIcon;

    [SerializeField] protected int _Cooldown;
    public int cooldown => _Cooldown;

    public int currentCooldown;

    [SerializeField] protected float _multiplier;
    public float multiplier => _multiplier;

    protected float additionalScalings = 0;

    protected int damage;
    public virtual float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        //check for crit
        //(Attack - EnemyDef + scalingMultiplier) * cdmg (if it crits) * multiplier
        if (IsCriticalHit(attacker.trueStats.critRate))
            damage = (int)Mathf.Round((attacker.trueStats.attack - attackee.trueStats.defense) * (attacker.trueStats.critDMG / 100) * (multiplier + additionalScalings));
        else
            damage = (int)Mathf.Round((attacker.trueStats.attack - attackee.trueStats.defense) * (multiplier + additionalScalings));

        return damage;
    }

    protected bool IsCriticalHit(float criticalChancePercentage)
    {
        float randomValue = Random.Range(0f, 100f); // Generate a random value between 0 and 100
        return randomValue <= criticalChancePercentage; // Check if the random value is less than or equal to the critical chance percentage
    }

    public virtual void Use(EntityBase attacker, EntityBase attackee)
    {
        attackee.TakeDamage(CalculateDamage(attacker, attackee));

        CombatManager.Instance.StartCoroutine(SkillAnimationCoroutine(attacker));
    }

    protected virtual Debuff InitDebuff(EntityBase attacker, EntityBase attackee, int duration, DebuffData debuffData)
    {
        Debuff debuff = new Debuff(attacker, attackee, duration);
        debuff.debuffData = debuffData;
        return debuff;
    }

    protected bool stayOnAnimation = false;
    IEnumerator SkillAnimationCoroutine(EntityBase attacker)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length);
        attacker.PostSkill(stayOnAnimation);
    }
}
