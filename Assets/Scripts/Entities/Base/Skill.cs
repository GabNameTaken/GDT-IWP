using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum SKILL_CODE
{
    S1,
    S2,
    S3,
    PASSIVE,
    PASSIVE2,
    NONE,
}

public class Skill : ScriptableObject
{
    public enum SKILL_TARGET_TEAM
    {
        ENEMY,
        ALLY,
        SELF,
        NONE
    }

    public enum SKILL_TARGETS
    {
        SINGLE_TARGET,
        ALL,
        ADJACENT,
        NONE,
        TOTAL
    }

    public SKILL_TARGET_TEAM targetTeam;
    public SKILL_TARGETS targets;

    [SerializeField] protected string _skillName;
    public string skillName => _skillName;

    [SerializeField] protected string _skillDesc;
    public string skillDesc => _skillDesc;

    [SerializeField] protected Sprite _skillIcon;
    public Sprite skillIcon => _skillIcon;

    [SerializeField] int _skillCost;
    public int skillCost => _skillCost;

    [SerializeField] protected string readyAnimationString;

    [SerializeField] protected SkillParticle skillParticle;

    [SerializeField] protected int _Cooldown;
    public int cooldown => _Cooldown;

    public int currentCooldown;

    [SerializeField] protected float _multiplier;
    public float multiplier => _multiplier;

    protected float additionalScalings = 0;

    protected int damage;

    public virtual void OnBattleStart()
    {
    }

    public virtual void OnBattleEnd()
    {
    }

    public virtual void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        if (attacker.weaponModel)
            attacker.weaponModel.AttachWeapon();

        PlayerTeamManager.Instance.UpdateSkillPoints(skillCost, true);

        CombatManager.Instance.StartCoroutine(SkillAnimationCoroutine(attacker, attackeeList));
    }

    protected virtual StatusEffect InitStatusEffect(EntityBase attacker, EntityBase attackee, int duration, StatusEffectData statusEffectData)
    {
        StatusEffect statusEffect = new StatusEffect(attacker, attackee, duration, statusEffectData);
        return statusEffect;
    }

    private System.Random random = new System.Random();
    protected virtual bool RunProbability(float probability)
    {
        // Generate a random value between 0 and 1
        float randomValue = (float)random.NextDouble();

        // Check if the random value is less than or equal to the probability
        if (randomValue <= probability)
        {
            // If the condition is met, then execute the line
            return true;
        }
        return false;
    }

    protected virtual IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        yield return null;

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        foreach (EntityBase attackee in attackeeList)
            if (!attackee.IsDead)
                attackee.TakeDamage(CalculateDamage(attacker, attackee), attacker.entity.element);

        ApplyStatusEffects(attacker, attackeeList);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.8f);

        attacker.PostSkill();
    }

    protected virtual void ApplyStatusEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {

    }

    public virtual float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        //check for crit
        //(Attack - EnemyDef + scalingMultiplier) * cdmg (if it crits) * multiplier
        if (IsCriticalHit(attacker.trueStats.critRate))
            damage = (int)Mathf.Round((attacker.trueStats.attack * (multiplier + additionalScalings) - attackee.trueStats.defense) * (attacker.trueStats.critDMG / 100));
        else
            damage = (int)Mathf.Round((attacker.trueStats.attack * (multiplier + additionalScalings) - attackee.trueStats.defense));

        if (damage <= 0)
            damage = 10;

        return damage;
    }

    protected bool IsCriticalHit(float criticalChancePercentage)
    {
        float randomValue = Random.Range(0f, 100f); // Generate a random value between 0 and 100
        return randomValue <= criticalChancePercentage; // Check if the random value is less than or equal to the critical chance percentage
    }

    protected Vector3 GetFrontPos(Vector3 attackerPos, Vector3 attackeePos, float setDistance)
    {
        float distance = (attackerPos - attackeePos).magnitude;
        Vector3 direction = (attackerPos - attackeePos).normalized;

        if (distance > setDistance)
        {
            Vector3 targetPos = attackeePos + direction * setDistance;
            return targetPos;
        }
        return attackeePos;
    }

    protected Quaternion GetQuaternionRotationToTarget(Vector3 attackerPos, Vector3 attackeePos)
    {
        // Calculate the direction vector towards the enemy
        Vector3 directionToEnemy = (attackeePos - attackerPos).normalized;

        // Calculate the rotation quaternion to look in that direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);

        return targetRotation;
    }

    public virtual void PlayReadyAnimation(EntityBase attacker)
    {
        if (attacker.animator.HasState(0, Animator.StringToHash(readyAnimationString)))
        {
            if (attacker.weaponModel)
                attacker.weaponModel.AttachWeapon();
            attacker.animator.Play(readyAnimationString);
        }
        else
        {
            if (attacker.weaponModel)
                attacker.weaponModel.RestWeapon();
            attacker.animator.Play("Idle");
        }
    }
}
