using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EntityBase : MonoBehaviour
{
    public Animator animator;

    public Entity entity;
    [SerializeField] BaseStats baseStats;
    [HideInInspector] public Stats trueStats;

    public float turnMeter;
    public bool isMoving = false;
    public bool isDead = false;

    public GameObject turnMeterUI;

    public List<EntityBase> listOfTargets;

    protected Vector3 originalPosition;
    protected Quaternion originalRotation;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        trueStats.health -= damage;
    }

    public void PostSkill()
    {
        animator.Play("Idle");
        if (transform.position != originalPosition)
        {
            transform.DOMove(originalPosition, 1, false).OnComplete(()=> CombatManager.Instance.EndTurn(this));
            transform.DORotate(originalRotation.eulerAngles, 1);
        }
        else
            CombatManager.Instance.EndTurn(this);
    }
}
