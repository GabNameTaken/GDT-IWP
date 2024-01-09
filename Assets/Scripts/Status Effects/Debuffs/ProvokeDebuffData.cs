using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Provoke")]
public class ProvokeDebuffData : StatusEffectData
{
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        if (!dest.asleep && !dest.unableToAct)
            dest.Provoked(source);
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
    }
}
