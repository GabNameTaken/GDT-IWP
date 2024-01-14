using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

[CreateAssetMenu(menuName ="Skills/Highest ATK Enemy TurnMeter Push")]
public class HighestATKEnemyTurnMeterPush : Skill
{
    [SerializeField] string animationStr;
    public float amount;
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play(animationStr);
        base.Use(attacker, attackeeList);
    }

    protected override void ApplyEffects(EntityBase attacker, List<EntityBase> attackeeList)
    {
        Enemy highestATK = CombatManager.Instance.EnemyParty
                            .Where(enemy => !enemy.IsDead)
                            .OrderByDescending(enemy => enemy.trueStats.attack)
                            .FirstOrDefault();
        highestATK.TurnMeter += amount;
    }

    public override float CalculateDamage(EntityBase attacker, EntityBase attackee)
    {
        return 0;
    }
}
