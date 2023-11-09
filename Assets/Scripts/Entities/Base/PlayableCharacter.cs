using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayableCharacter : EntityBase
{
    [SerializeField] SkillSet skillSet;
    bool attacking = false;
    public int currentTargetNum = 0;
    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
    }

    private void Update()
    {
        if (!isMoving || attacking) return;

        SkillSelectionCheck();
        SelectTarget();
        if (Input.GetKeyDown(KeyCode.Space) && currentSkillCode != SKILL_CODE.NONE)
        {
            UseSkill(currentSkillCode);
        }
    }

    enum SKILL_CODE
    {
        S1,
        S2,
        S3,
        NONE,
    }
    SKILL_CODE currentSkillCode = SKILL_CODE.NONE;

    int keyIndex = (int)SKILL_CODE.NONE;
    void SkillSelectionCheck()
    {
        // Define an array to store the KeyCode values for your skills
        KeyCode[] skillKeys = { KeyCode.Q, KeyCode.E, KeyCode.R };

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                keyIndex = i;
                if (currentSkillCode == (SKILL_CODE)keyIndex)
                    UseSkill(currentSkillCode);
            }
        }
        if (currentSkillCode == SKILL_CODE.NONE)
        {
            currentSkillCode = (SKILL_CODE)keyIndex;
        }
        else if (currentSkillCode < SKILL_CODE.NONE)
        {
            if ((int)currentSkillCode != keyIndex)
                currentSkillCode = SKILL_CODE.NONE;
        }
    }

    void SelectTarget()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentTargetNum > 0)
                currentTargetNum--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentTargetNum < listOfTargets.Count)
                currentTargetNum++;
        }
    }

    void UseSkill(SKILL_CODE skill)
    {
        attacking = true;
        switch (skill)
        {
            case SKILL_CODE.S1:
                {
                    Attack(skillSet.S1);
                    break;
                }
            case SKILL_CODE.S2:
                {
                    Attack(skillSet.S2);
                    break;
                }
            case SKILL_CODE.S3:
                {
                    Attack(skillSet.S3);
                    break;
                }
        }
    }

    void Attack(Skill skill)
    {
        skill.Use(this, listOfTargets[currentTargetNum]);
        currentTargetNum = 0;
        keyIndex = (int)SKILL_CODE.NONE;
        attacking = false;
    }
}