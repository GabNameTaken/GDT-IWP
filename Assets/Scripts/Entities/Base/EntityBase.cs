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
        if (damage > 0)
            animator.Play("GetHit");
        trueStats.health -= damage;
        if (trueStats.health <= 0)
        {
            trueStats.health = 0;
            isDead = true;
        }
        else if (trueStats.health > trueStats.maxHealth)
            trueStats.health = trueStats.maxHealth;

        CombatUIManager.Instance.UpdateHealth(this);
    }

    void OnDeath()
    {
        animator.Play("Dead");
        CombatManager.Instance.Pause(this, true);
        //StartCoroutine(DeathAnimationCoroutine());
        Death();
    }

    IEnumerator DeathAnimationCoroutine()
    {
        yield return null;

        Camera.main.transform.DOKill();
        CameraManager.Instance.MoveCamera(gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0f);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        CombatManager.Instance.Pause(this, false);
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
