using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    [SerializeField] protected string _skillName;
    public string skillName => _skillName;

    [SerializeField] protected string _skillDesc;
    public string skillDesc => _skillDesc;

    [SerializeField] protected Sprite _skillIcon;
    public Sprite skillIcon => _skillIcon;

    [SerializeField] protected int _Cooldown;
    public int cooldown => _Cooldown;

    [SerializeField] protected float _multiplier;
    public float multiplier => _multiplier;

    public List<Entity> _listOfTargets;
    
    protected int damage;
    protected virtual int Use(Stats currentStats, Stats enemyStats)
    {
        //check for crit
        //(Attack - EnemyDef + scalingMultiplier) * cdmg (if it crits) * multiplier
        if (IsCriticalHit(currentStats.critRate))
            damage = (int)Mathf.Round((currentStats.attack - enemyStats.defense) * (currentStats.critDMG / 100) * multiplier);
        else
            damage = (int)Mathf.Round(currentStats.attack - enemyStats.defense);  
        return damage;
    }

    protected bool IsCriticalHit(float criticalChancePercentage)
    {
        float randomValue = Random.Range(0f, 100f); // Generate a random value between 0 and 100
        return randomValue <= criticalChancePercentage; // Check if the random value is less than or equal to the critical chance percentage
    }
}
