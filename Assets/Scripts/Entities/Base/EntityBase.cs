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
    [HideInInspector] public Stats trueStats;

    [SerializeField] protected SkillSet _skillSet;
    public SkillSet skillSet => _skillSet;

    public List<StatusEffect> statusEffectList = new();
    public EntityInfoUI entityInfoUI;

    public System.Action<float> TurnMeterChangedEvent;
    private float turnMeter;
    public float TurnMeter 
    { 
        get { return turnMeter; } 
        set { turnMeter = value; TurnMeterChangedEvent?.Invoke(turnMeter); } 
    }

    public float excessTurnMeter;

    public bool asleep { get; private set; } = false;
    public bool unableToAct = false;
    public bool isMoving = false;

    [HideInInspector] public float damageTaken;
    public event System.Action<bool> IsDeadChangedEvent;
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; IsDeadChangedEvent?.Invoke(isDead); }
    }

    protected bool attacking = false;

    public GameObject turnMeterUI;

    public ParticleSystem hitParticleSystem;
    public SkillParticle healingParticlePrefab;

    public cakeslice.Outline outline;
    public List<EntityBase> listOfTargets;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    [SerializeField] LayerMask terrainLayer = 6;

    private void FixedUpdate()
    {
        StickYToTerrain();
    }

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

    protected virtual void Awake()
    {

    }

    public virtual void TakeDamage(float damage, Element element)
    {
        damageTaken = damage;
        if (damageTaken > 0)
        {
            CombatManager.Instance.CallEntityTakeDamageEvent(this);
            if (asleep)
            {
                StatusEffect sleep = statusEffectList.FirstOrDefault(effect => effect.StatusEffectData.statusEffectName == "Sleep");
                RemoveStatusEffect(sleep);
            }
            animator.Play("GetHit");
            if (element && hitParticleSystem)
            {
                hitParticleSystem.startColor = element.elementColor;
                hitParticleSystem.Play();
            }
        }
        else if (damageTaken < 0)
        {
            SkillParticle healingParticle = Instantiate(healingParticlePrefab, transform);
            healingParticle.Play();
        }
        
        trueStats.health -= damageTaken;
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
        entityInfoUI.UpdateSkillUI();
    }

    public void OnDeath()
    {
        isDead = true;
        StartCoroutine(DeathAnimationCoroutine());
        Death();

        CombatManager combatManager = CombatManager.Instance;

        //List<EntityBase> provokedEntities = combatManager.entitiesOnField
        //    .Where(entity => entity.statusEffectList
        //        .Any(debuff => debuff.StatusEffectData.statusEffectName == "Provoke" && debuff.giver == this))
        //    .ToList();

        //foreach (EntityBase provokedEntity in provokedEntities)
        //{
        //    foreach (StatusEffect statusEffect in provokedEntity.statusEffectList)
        //    {
        //        if (statusEffect.StatusEffectData.statusEffectName == "Provoke" && statusEffect.giver == this)
        //        {
        //            provokedEntity.RemoveStatusEffect(statusEffect);
        //        }
        //    }
        //}
    }

    IEnumerator DeathAnimationCoroutine()
    {
        animator.Play("Dead");

        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        CombatManager combatManager = CombatManager.Instance;
        combatManager.CallEntityDeadEvent(this);

        if (this && isDead)
        {
            if (transform.Find("Main Camera"))
                Camera.main.transform.SetParent(MapManager.Instance.currentMap.transform.Find("CombatSetup").transform);
            gameObject.SetActive(false);
            turnMeterUI.SetActive(false);
            if (isMoving)
                combatManager.EndTurn(this);
        }
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

        //List<StatusEffect> buffList = new(statusEffectList.Where((a) => a.StatusEffectData.type == STATUS_EFFECT_TYPE.BUFF));
        //foreach (StatusEffect statusEffect in buffList)
        //    statusEffect.ApplyEffect();

        unableToAct = false;

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
        CombatManager.Instance.CallEntityStartTurnEvent(this);

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        foreach (Skill skill in skillSet.SkillDict.Values)
        {
            if (skill.currentCooldown > 0)
                skill.currentCooldown--;
        }
        if (GetComponent<PlayableCharacter>())
            CombatUIManager.Instance.DisplaySkillCooldown(skillSet);
        entityInfoUI.UpdateSkillUI();
        
        //play animation
        if (animator.HasState(0, Animator.StringToHash("Ready")))
            animator.Play("Ready");
        else
            animator.Play("Idle");

        if (!isDead)
            StartCoroutine(StartingTurn());
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (!statusEffect.StatusEffectData)
        {
            return;
        }
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
        entityInfoUI.statusEffectUI.RemoveStatus(statusEffect.icon);
    }

    protected IEnumerator StartingTurn()
    {
        //List<StatusEffect> debuffList = new(statusEffectList.Where((a) => a.StatusEffectData.type == STATUS_EFFECT_TYPE.DEBUFF));
        List<StatusEffect> tempStatusEffectList = new(statusEffectList);
        foreach (StatusEffect statusEffect in tempStatusEffectList)
            statusEffect.ApplyEffect();

        yield return new WaitForSeconds(1f);

        if (!unableToAct)
            StartTurn();
        else if (!attacking)
            PostSkill();
    }

    protected virtual void StartTurn()
    {
        isMoving = true;
    }

    public virtual void Provoked(EntityBase provoker)
    {
        attacking = true;
        unableToAct = true;
        listOfTargets.Clear();
        listOfTargets.Add(provoker);
        skillSet.SkillDict[SKILL_CODE.S1].Use(this, listOfTargets);
    }

    public void Sleep(bool sleep)
    {
        if (sleep)
        {
            asleep = sleep;
            animator.Play("Dizzy");
        }
        else
        {
            asleep = sleep;
            animator.Play("Idle");
        }
    }

    public bool ContainsSkill(Skill skill)
    {
        return skillSet.SkillDict.ContainsValue(skill);
    }

    float raycastHeight = 3f;
    private void StickYToTerrain()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * raycastHeight * 2f;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastHeight * 10f, terrainLayer))
        {
            Debug.DrawLine(raycastOrigin, hit.point, Color.green, 2f);

            // Calculate the new position with the detected terrain height.
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;

            // Limit the Y position to prevent the player from falling through the terrain.
            float minHeight = Mathf.Min(hit.point.y + 0.0001f, 3.5f);
            newPos.y = Mathf.Clamp(newPos.y, minHeight, Mathf.Infinity);

            // Apply the new position to the player.
            transform.position = newPos;
        }
        else
        {
            Debug.Log("Raycast did not hit terrain.");
        }
    }
}
