using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff
{
    public DebuffData debuffData;
    public int duration;
    protected int remainingDuration;

    public EntityBase giver;
    public EntityBase receiver;

    public Debuff(EntityBase _giver, EntityBase _receiver, int _duration)
    {
        giver = _giver;
        receiver = _receiver;
        duration = _duration;
        remainingDuration = duration;
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
        if (remainingDuration <= 0)
            receiver.debuffList.Remove(this);
    }
}
