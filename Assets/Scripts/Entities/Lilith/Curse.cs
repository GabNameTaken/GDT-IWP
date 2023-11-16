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
            InitPoison(attacker, enemy);
            enemy.debuffList.Add(poisonDebuff);
        }
    }

    void InitPoison(EntityBase attacker, EntityBase attackee)
    {
        poisonDebuff = new Debuff(attacker, attackee, 2);
        poisonDebuff.debuffData = poisonDebuffData;
    }
} 
