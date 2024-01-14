using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Debuffs/Curse")]
public class CurseDebuffData : StatusEffectData
{
    public override void OnStatusEffectAdd(EntityBase source, EntityBase dest)
    {
        SkillParticle particle = Instantiate(particlePrefab, dest.transform);
        particle.Play();
        foreach (Enemy enemy in CombatManager.Instance.EnemyParty)
        {
            enemy.lockedOnTarget = dest;
        }
    }

    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        
    }

    public override void OnStatusEffectRemove(EntityBase source, EntityBase dest)
    {
        foreach (Enemy enemy in CombatManager.Instance.EnemyParty)
        {
            if (enemy.lockedOnTarget == dest)
                enemy.lockedOnTarget = null;
        }
    }
}
