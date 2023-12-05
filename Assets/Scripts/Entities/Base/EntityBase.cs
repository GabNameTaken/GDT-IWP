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

    public Entity entity;
    [SerializeField] public BaseStats baseStats { get; private set; }
    [HideInInspector] public Stats trueStats;

    [SerializeField] protected SkillSet skillSet;

    protected List<StatusEffect> statusEffectList = new();
    public StatusEffectUI statusEffectUI;

    public float turnMeter;
    public bool isMoving = false;
    public bool isDead = false;
    protected bool attacking = false;

    public GameObject turnMeterUI;

    public ParticleSystem hitParticleSystem;
    public cakeslice.Outline outline;
    public List<EntityBase> listOfTargets;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
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

        if (isMoving)
            CombatManager.Instance.EndTurn(this);
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

    public void PostSkill(bool remainInAnimation)
    {
        if (!remainInAnimation)
        {
            animator.Play("Idle");
            if (weaponModel)
                weaponModel.RestWeapon();
        }
        

        attacking = false;
        if (transform.position != originalPosition)
        {
            transform.DOMove(originalPosition, 1, false).OnComplete(() => CombatManager.Instance.EndTurn(this));
            transform.DORotate(originalRotation.eulerAngles, 1);
        }
        else
            CombatManager.Instance.EndTurn(this);
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

        List<StatusEffect> debuffList = new(statusEffectList.Where((a)=>a.StatusEffectData.type == STATUS_EFFECT_TYPE.DEBUFF));
        foreach (StatusEffect statusEffect in debuffList)
            statusEffect.ApplyEffect();

        StartCoroutine(StartingTurn());
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (!statusEffect.StatusEffectData.stackable)
            statusEffectList.RemoveAll(existingStatus => existingStatus.StatusEffectData == statusEffect.StatusEffectData);

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
}
