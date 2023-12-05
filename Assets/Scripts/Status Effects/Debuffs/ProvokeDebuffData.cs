using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Provoke")]
public class ProvokeDebuffData : StatusEffectData
{
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    { 
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.Provoked(source);
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
    }
}
