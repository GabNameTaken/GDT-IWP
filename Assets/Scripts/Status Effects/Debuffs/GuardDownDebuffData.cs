using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Guard Down")]
public class GuardDownDebuffData : StatusEffectData
{
    [SerializeField] float defDecreaseMultiplier;
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        dest.trueStats.defense /= (defDecreaseMultiplier / 100f);

        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        dest.trueStats.defense *= (defDecreaseMultiplier / 100f);
    }
}
