using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using cakeslice;

public enum CAMERA_POSITIONS
{
    HIGH_BACK,
    LOW_BACK,
    LOW_FRONT_SELF,
    HIGH_FRONT_SELF,
    PLAYER_TEAM_BACK,
    PLAYER_TEAM_FRONT,
    NONE,
}

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

        if (transform.position != originalPosition)
        {
            transform.DOMove(originalPosition, 1, false).OnComplete(() => CombatManager.Instance.EndTurn(this));
            transform.DORotate(originalRotation.eulerAngles, 1);
        }
        else
        {
            CombatManager.Instance.EndTurn(this);
            attacking = false;
        }
    }

    public virtual void TakeTurn()
    {
        //Debug.Log(name + " taken turn");
        isMoving = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        
        //play animation
        if (animator.HasState(0, Animator.StringToHash("Ready")))
            animator.Play("Ready");
        else
            animator.Play("Idle");

        for (int i = 0; i < debuffList.Count; i++)
            debuffList[i].ApplyEffect();
    }

    public virtual void Provoked(EntityBase provoker)
    {
        attacking = true;
        skillSet.SkillDict[SKILL_CODE.S1].Use(this, provoker);
        listOfTargets.Clear();
    }
}
