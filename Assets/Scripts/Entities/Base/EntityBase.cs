using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using cakeslice;

public class EntityBase : MonoBehaviour
{
    public Animator animator;

    public Entity entity;
    [SerializeField] BaseStats baseStats;
    [HideInInspector] public Stats trueStats;

    [SerializeField] protected SkillSet skillSet;

    public List<Debuff> debuffList = new();
    public StatusEffectUI statusEffectUI;

    public float turnMeter;
    public bool isMoving = false;
    public bool isDead = false;
    protected bool attacking = false;

    public GameObject turnMeterUI;

    public cakeslice.Outline outline;
    public List<EntityBase> listOfTargets;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        trueStats.health -= damage;
        if (trueStats.health <= 0)
        {
            trueStats.health = 0;
            OnDeath();
        }
        CombatUIManager.Instance.UpdateHealth(this);
    }

    void OnDeath()
    {
        isDead = true;
        animator.Play("Dead");
        //StartCoroutine(DeathAnimationCoroutine());
        Death();
    }

    //IEnumerator DeathAnimationCoroutine()
    //{
    //    yield return null;

    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    //}

    void Death()
    {
        //delink everything
    }

    public void PostSkill(bool remainInAnimation)
    {
        if (!remainInAnimation)
            animator.Play("Idle");

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
        
        //play animation
        if (animator.HasState(0, Animator.StringToHash("Ready")))
            animator.Play("Ready");
        else
            animator.Play("Idle");

        List<Debuff> tempList = new(debuffList);
        foreach (Debuff debuff in tempList)
            debuff.ApplyEffect();

        StartCoroutine(StartingTurn());
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
