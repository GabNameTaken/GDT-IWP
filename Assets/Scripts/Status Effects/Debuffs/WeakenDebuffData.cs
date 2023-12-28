using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Weaken")]
public class WeakenDebuffData : StatusEffectData
{
    [SerializeField] float weakenAttackMultiplier;
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack /= (weakenAttackMultiplier / 100f);

        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack *= (weakenAttackMultiplier / 100f);
    }
}
