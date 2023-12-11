using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using cakeslice;
using System.Linq;

public class EntityBase : MonoBehaviour
{
    public Animator animator;
    public WeaponModel weaponModel;
    public GameObject model;

    public Entity entity;
    [SerializeField] public BaseStats baseStats { get; private set; }
    [HideInInspector] public Stats trueStats;

    [SerializeField] protected SkillSet skillSet;

    protected List<StatusEffect> statusEffectList = new();
    public StatusEffectUI statusEffectUI;

    public float turnMeter;
    public float excessTurnMeter;
    public bool isMoving = false;
    public bool isDead = false;
    protected bool attacking = false;

    public GameObject turnMeterUI;

    public ParticleSystem hitParticleSystem;
    public cakeslice.Outline outline;
    public List<EntityBase> listOfTargets;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    public void OnBattleStart()
    {
        foreach (Skill skill in skillSet.SkillDict.Values)
            skill.OnBattleStart();
    }

    public void OnBattleEnd()
    {
        foreach (Skill skill in skillSet.SkillDict.Values)
            skill.OnBattleEnd();
    }

    public virtual void TakeDamage(float damage, Element element)
    {
        if (damage > 0)
        {
            animator.Play("GetHit");
            if (element)
            {
                hitParticleSystem.startColor = element.elementColor;
                hitParticleSystem.Play();
            }
        }
        
        trueStats.health -= damage;
        if (trueStats.health <= 0)
        {
            trueStats.health = 0;
            OnDeath();
        }
        else if (trueStats.health > trueStats.maxHealth)
            trueStats.health = trueStats.maxHealth;

        CombatUIManager.Instance.UpdateHealth(this);
    }

    protected virtual void Attack(Skill skill)
    {
        skill.currentCooldown = skill.cooldown;
    }

    public void OnDeath()
    {
        isDead = true;
        StartCoroutine(DeathAnimationCoroutine());
        Death();

        CombatManager combatManager = CombatManager.Instance;
        combatManager.CallEntityDeadEvent(this);
        if (isMoving)
            combatManager.EndTurn(this);
    }

    IEnumerator DeathAnimationCoroutine()
    {
        animator.Play("Dead");

        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        gameObject.SetActive(false);
    }

    void Death()
    {
        //delink everything
    }

    public void PostSkill()
    {
        animator.Play("Idle");
        if (weaponModel)
            weaponModel.RestWeapon();

        List<StatusEffect> buffList = new(statusEffectList.Where((a) => a.StatusEffectData.type == STATUS_EFFECT_TYPE.BUFF));
        foreach (StatusEffect statusEffect in buffList)
            statusEffect.ApplyEffect();

        if (transform.position != originalPosition)
        {
            transform.DOMove(originalPosition, 1, false).OnComplete(() =>
            {
                CombatManager.Instance.EndTurn(this);
                attacking = false;
            });
            transform.DORotate(originalRotation.eulerAngles, 1);
        }
        else
        {
            attacking = false;
            CombatManager.Instance.EndTurn(this);
        }
    }

    public virtual void TakeTurn()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        for (int i = 0; i < skillSet.SkillDict.Count; i++)
        {
            if (skillSet.SkillDict[(SKILL_CODE)i].currentCooldown > 0)
                skillSet.SkillDict[(SKILL_CODE)i].currentCooldown--;
        }
        CombatUIManager.Instance.DisplaySkillCooldown(skillSet);
        
        //play animation
        if (animator.HasState(0, Animator.StringToHash("Ready")))
            animator.Play("Ready");
        else
            animator.Play("Idle");

        

        StartCoroutine(StartingTurn());
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (!statusEffect.StatusEffectData.stackable)
            statusEffectList
                .Where(existingStatus => existingStatus.StatusEffectData.Equals(statusEffect.StatusEffectData))
                .ToList()
                .ForEach(existingStatus => RemoveStatusEffect(existingStatus));

        statusEffectList.Add(statusEffect);
        statusEffect.StatusEffectData.OnStatusEffectAdd(statusEffect.giver, statusEffect.receiver);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.StatusEffectData.OnStatusEffectRemove(statusEffect.giver, statusEffect.receiver);
        statusEffectList.Remove(statusEffect);
    }

    protected IEnumerator StartingTurn()
    {
        List<StatusEffect> debuffList = new(statusEffectList.Where((a) => a.StatusEffectData.type == STATUS_EFFECT_TYPE.DEBUFF));
        foreach (StatusEffect statusEffect in debuffList)
            statusEffect.ApplyEffect();

        yield return new WaitForSeconds(1f);

        StartTurn();
    }

    protected virtual void StartTurn()
    {
        isMoving = true;
    }

    public virtual void Provoked(EntityBase provoker)
    {
        attacking = true;
        skillSet.SkillDict[SKILL_CODE.S1].Use(this, provoker);
        listOfTargets.Clear();
    }

    public bool ContainsSkill(Skill skill)
    {
        return skillSet.SkillDict.ContainsValue(skill);
    }
}
