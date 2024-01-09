using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Sleep")]
public class SleepDebuffData : StatusEffectData
{
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
        dest.asleep = true;
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.unableToAct = true;
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.asleep = false;
    }
}
