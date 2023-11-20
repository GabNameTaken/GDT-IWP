using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Debuff
{
    private StatusEffectUI statusEffectUI;
    public GameObject icon;

    DebuffData debuffData;
    public DebuffData data => debuffData;
    public int duration;
    protected int remainingDuration;

    public EntityBase giver;
    public EntityBase receiver;

    public Debuff(EntityBase _giver, EntityBase _receiver, int _duration, DebuffData debuffData)
    {
        giver = _giver;
        receiver = _receiver;
        duration = _duration;
        remainingDuration = duration;
        this.debuffData = debuffData;

        statusEffectUI = receiver.statusEffectUI;
        statusEffectUI.OnAddStatus(this, remainingDuration);
    }

    public virtual void ApplyEffect()
    {
        // Apply the debuff's effects to the character
        debuffData.ApplyEffect(giver, receiver);
        DecreaseDuration();
    }

    protected void DecreaseDuration()
    {
        remainingDuration--;
        statusEffectUI.UpdateStatus(icon, remainingDuration);

        if (remainingDuration <= 0)
        {
            debuffData.OnRemove(giver, receiver);
            receiver.debuffList.Remove(this);
        }
    }
}