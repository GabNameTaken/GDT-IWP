using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base/Stats")]
public class BaseStats : ScriptableObject
{
    [SerializeField] Stats stats;
    public Stats Stats => stats;
}

[System.Serializable]
public class Stats
{
    public float maxHealth, health, defense, attack;
    public int speed, critRate = 15, critDMG = 150;

    public Stats(Stats stats) 
    {
        maxHealth = stats.maxHealth;
        health = stats.health;
        defense = stats.defense;
        attack = stats.attack;
        speed = stats.speed;
        critRate = stats.critRate;
        critDMG = stats.critDMG;
    }
}
