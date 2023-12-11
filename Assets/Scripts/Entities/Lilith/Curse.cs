using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Curse")]
public class Curse : Skill
{
    [SerializeField] PoisonDebuffData poisonDebuffData;
    float poisonChance;

    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("CurseAttack");
        base.Use(attacker, CombatManager.Instance.enemyParty.ConvertAll(entity => (EntityBase)entity));
        foreach(Enemy enemy in CombatManager.Instance.enemyParty)
        {
            if (!enemy.isDead)
            {
                SkillParticle particle = Instantiate(skillParticle, enemy.transform);
                particle.Play();
                enemy.AddStatusEffect(InitStatusEffect(attacker, enemy, 2, poisonDebuffData));
            }
        }
    }
} 
