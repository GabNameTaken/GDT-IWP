using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base/Stats")]
public class Stats : ScriptableObject
{
    public float health, defense, attack;
    public int speed, critRate = 15, critDMG = 150;
}
