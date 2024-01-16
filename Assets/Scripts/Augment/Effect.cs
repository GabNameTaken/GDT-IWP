using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

abstract public class Effect : ScriptableObject
{
}

abstract public class LastingEffect : Effect
{
    public abstract void RegisterEffect();
    public abstract void UnregisterEffect();
}

abstract public class EventEffect<T> : Effect
{
    public abstract void RegisterEffect(T param);
}

abstract public class SkillEffect : EventEffect<Skill>
{
    public override void RegisterEffect(Skill skill)
    {
    }
}

abstract public class KillEffect : EventEffect<EntityBase>
{
    public override void RegisterEffect(EntityBase deadEntity)
    {
    }
}
