using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    [SerializeField] SkillSet skillSet;
    [SerializeField] Entity character;
    public Stats stats;
    public int turnMeter;

    private void Awake()
    {
        skillSet = character.baseSkillSet;
        stats = character.baseStats;
    }


}