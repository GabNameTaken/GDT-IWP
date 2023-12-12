using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Attack")]
public class AttackBuff : StatusEffectData
{
    [SerializeField] float attackBuffMultiplier;

    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack *= (attackBuffMultiplier / 100f);

        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.attack /= (attackBuffMultiplier / 100f);
    }
}
