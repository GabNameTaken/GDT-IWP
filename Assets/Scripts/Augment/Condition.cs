using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Condition : ScriptableObject
{
    [SerializeField] List<StateCheck> stateChecks = new List<StateCheck>(); // Additional on-going game requirements

    bool hasInit = false;
    public virtual void Init(bool stackable)
    {
        if (!stackable)
        {
            if (hasInit) return;
            hasInit = true;
        }
    }

    public virtual void Cleanup(bool stackable)
    {
        if (!stackable)
        {
            if (!hasInit) return;
            hasInit = false;
        }
    }

    protected bool Check()
    {
        if (stateChecks.Any(stateCheck => !stateCheck.CheckState())) return false;
        return true;
    }
}

public abstract class EventCondition<T> : Condition
{
    [SerializeField] protected EventEffect<T> Effect;

    protected abstract void OnGameEvent(T eventData);

    public override void Init(bool stackable)
    {
        base.Init(stackable);
        AttachEvent();
    }

    public override void Cleanup(bool stackable)
    {
        base.Cleanup(stackable);
        DetachEvent();
    }

    abstract public void AttachEvent();
    abstract public void DetachEvent();
}
