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
        giver = _giver;
        receiver = _receiver;
        duration = _duration;
        remainingDuration = duration;
        this.statusEffectData = statusEffectData;

        CombatManager.Instance.StartCoroutine(UpdateUI());
    }


    protected IEnumerator UpdateUI()
    {
        yield return null;

        yield return new WaitForSeconds(giver.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        statusEffectUI = receiver.statusEffectUI;
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
            receiver.RemoveStatusEffect(this);
    }
}
