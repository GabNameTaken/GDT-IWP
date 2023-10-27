using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : EntityBase
{
    [SerializeField] SkillSet skillSet;
    [SerializeField] Entity character;

    private void Awake()
    {
        skillSet = character.baseSkillSet;
        stats = character.baseStats;
    }

    private void Update()
    {
        if (!isMoving) return;

        
    }
}