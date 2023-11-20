using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Curse")]
public class Curse : Skill
{
    [SerializeField] PoisonDebuffData poisonDebuffData;
    private Debuff poisonDebuff;
    int poisonChance;
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("CurseAttack");
        base.Use(attacker, CombatManager.Instance.enemyParty.ConvertAll(entity => (EntityBase)entity));
        foreach(Enemy enemy in CombatManager.Instance.enemyParty)
        {
            poisonDebuff = InitDebuff(attacker, enemy, 2, poisonDebuffData);
            enemy.debuffList.Add(poisonDebuff);
        }
    }
} 
