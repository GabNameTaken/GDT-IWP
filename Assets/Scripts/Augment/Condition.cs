using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Condition : ScriptableObject
{
    [SerializeField] List<StateCheck> stateChecks = new List<StateCheck>(); // Additional on-going game requirements

    bool hasInit = false;
    public virtual void Init()
    {
        if (hasInit) return;
        hasInit = true;
    }

    public virtual void Cleanup()
    {
        if (!hasInit) return;
        hasInit = false;
    }

    protected bool Check()
    {
        if (stateChecks.Any(stateCheck => !stateCheck.CheckState())) return false;
        return true;
    }
}
public abstract class EventCondition<T> : Condition
{
    protected EventEffect<T> Effect;

    protected abstract void OnGameEvent(T eventData);

    public override void Init()
    {
        base.Init();
        AttachEvent();
    }

    public override void Cleanup()
    {
        base.Cleanup();
        DetachEvent();
    }

    abstract public void AttachEvent();
    abstract public void DetachEvent();
}

[CreateAssetMenu(fileName = "SkillCondition", menuName = "Conditions/SkillCondition")]
public class SkillCondition : EventCondition<Skill>
{
    public override void AttachEvent()
    {
    }

    public override void DetachEvent()
    {
    }

    protected override void OnGameEvent(Skill skill)
    {
        if (!Check()) return;
        Effect.RegisterEffect(skill);
    }
}

[CreateAssetMenu(fileName = "KillCondition", menuName = "Conditions/KillCondition")]
public class KillCondition : EventCondition<EntityBase>
{
    public override void AttachEvent()
    {
        CombatManager.Instance.EntityDeadEvent += OnGameEvent;
    }

    public override void DetachEvent()
    {
        CombatManager.Instance.EntityDeadEvent -= OnGameEvent;
    }

    protected override void OnGameEvent(EntityBase deadEntity)
    {
        if (!Check()) return;
        Effect.RegisterEffect(deadEntity);
    }
}