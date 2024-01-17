using System.Collections;
using System.Collections.Generic;
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
    public abstract override void RegisterEffect(Skill skill);
}

abstract public class HitEffect : EventEffect<EntityBase>
{
    public abstract override void RegisterEffect(EntityBase hitEntity);
}

abstract public class KillEffect : EventEffect<EntityBase>
{
    public abstract override void RegisterEffect(EntityBase deadEntity);
}

abstract public class TurnEndEffect : EventEffect<EntityBase>
{
    public abstract override void RegisterEffect(EntityBase turnEndedEntity);
}
