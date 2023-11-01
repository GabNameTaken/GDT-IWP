using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : EntityBase
{
    [SerializeField] SkillSet skillSet;
    bool selectedS1, selectedS2, selectedS3, attacking = false;
    
    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
    }

    private void Update()
    {
        if (!isMoving) return;

        SkillSelectionCheck();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedS1)
            {
                selectedS1 = false;
                attacking = true;
                skillSet.S1.Use(this, listOfTargets[0]);
                CombatUIManager.Instance.UpdatePlayerTeamHealth();
            }
        }
        if (attacking)
        {
            if (!animator.IsInTransition(0))
            {
                isMoving = false;
                attacking = false;
                if (!isDead)
                    animator.Play("Idle");
                CombatManager.Instance.EndTurn(this);
            }
        }
    }

    bool SkillSelectionCheck()
    {
        // Define an array to store the KeyCode values for your skills
        KeyCode[] skillKeys = { KeyCode.E, KeyCode.Q, KeyCode.R };

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                if (selectedS1)
                {
                    selectedS1 = !(i == 0);

                    selectedS2 = i == 1 ? true : false;
                    selectedS3 = i == 2 ? true : false;
                }
                else if (selectedS2)
                {
                    selectedS2 = i == 1 ? false : true;

                    selectedS1 = i == 0 ? true : false;
                    selectedS3 = i == 2 ? true : false;
                }
                else if (selectedS3)
                {
                    selectedS3 = i == 2 ? false : true;

                    selectedS2 = i == 1 ? true : false;
                    selectedS1 = i == 0 ? true : false;
                }
                else
                {
                    selectedS1 = i == 0 ? true : false;
                    selectedS2 = i == 1 ? true : false;
                    selectedS3 = i == 2 ? true : false;
                }
                return true;
            }
        }

        return false;
    }
}