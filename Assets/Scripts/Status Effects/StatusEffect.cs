using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffect
{
    StatusEffectData statusEffectData;
    public StatusEffectData StatusEffectData => statusEffectData;

    protected StatusEffectUI statusEffectUI;
    public GameObject icon;

    public int duration;
    public int remainingDuration;

    public EntityBase giver;
    public EntityBase receiver;

    public StatusEffect(EntityBase _giver, EntityBase _receiver, int _duration, StatusEffectData statusEffectData)
    {
        if (_receiver.GetComponent<Enemy>() && _receiver.GetComponent<Enemy>().isBoss && statusEffectData.bossImmune)
        {
            //show some resist text or something
            return;
        }
        giver = _giver;
        receiver = _receiver;
        duration = _duration;
        remainingDuration = duration;
        this.statusEffectData = statusEffectData;

        if (statusEffectData.linkedToSource)
            CombatManager.Instance.EntityDeadEvent += CheckSource;

        statusEffectUI = receiver.entityInfoUI.statusEffectUI;
        statusEffectUI.OnAddStatus(this, remainingDuration);
    }

    public virtual void ApplyEffect()
    {
        // Apply the debuff's effects to the character
        statusEffectData.ApplyEffect(giver, receiver);
        DecreaseDuration();
    }

    protected void DecreaseDuration()
    {
        remainingDuration--;
        statusEffectUI.UpdateStatus(icon, remainingDuration);
        if (remainingDuration <= 0)
        {
            if (statusEffectData.linkedToSource)
                CombatManager.Instance.EntityDeadEvent -= CheckSource;
            receiver.RemoveStatusEffect(this);
        }
    }

    void CheckSource(EntityBase entity)
    {
        if (entity != giver && !entity.IsDead)
            return;
        CombatManager.Instance.EntityDeadEvent -= CheckSource;
        receiver.RemoveStatusEffect(this);
    }
}
