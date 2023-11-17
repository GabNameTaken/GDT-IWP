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
        foreach(Enemy enemy in CombatManager.Instance.enemyParty)
        {
            base.Use(attacker, enemy);
            poisonDebuff = InitDebuff(attacker, enemy, 2, poisonDebuffData);
            enemy.debuffList.Add(poisonDebuff);
        }
    }
} 
