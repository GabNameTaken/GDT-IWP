using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class StatusEffectData : ScriptableObject
{
    [SerializeField] string _name;
    public string statusEffectName => _name;

    [SerializeField] STATUS_EFFECT_TYPE _type;
    public STATUS_EFFECT_TYPE type => _type;

    [SerializeField] string _desc;
    public string desc => _desc;

    [SerializeField] Sprite _icon;
    public Sprite icon => _icon;

    [SerializeField] bool _stackable;
    public bool stackable => _stackable;

    [SerializeField] float _multiplier;
    public float multiplier => _multiplier;

    public abstract void OnStatusEffectAdd(EntityBase source, EntityBase dest);
    public abstract void ApplyEffect(EntityBase source, EntityBase dest);
    public abstract void OnStatusEffectRemove(EntityBase source, EntityBase dest);
}

public enum STATUS_EFFECT_TYPE
{
    BUFF,
    DEBUFF
}
