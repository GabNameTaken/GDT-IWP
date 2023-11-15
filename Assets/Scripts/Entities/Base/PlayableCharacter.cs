using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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

        SelectTargetInput();
        SkillSelectionCheck();
        if (Input.GetKeyDown(KeyCode.Space) && currentSkillCode != SKILL_CODE.NONE)
        {
            UseSkill(currentSkillCode);
        }
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

                if ((SKILL_CODE)keyIndex != SKILL_CODE.NONE)
                    SelectTargets(skillSet.SkillDict[(SKILL_CODE)keyIndex].targets, false);
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

    void SelectTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentTargetNum > 0)
                currentTargetNum--;
            if (currentSkillCode != SKILL_CODE.NONE)
                SelectTargets(skillSet.SkillDict[currentSkillCode].targets,false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentTargetNum < listOfTargets.Count - 1)
                currentTargetNum++;
            if (currentSkillCode != SKILL_CODE.NONE)
                SelectTargets(skillSet.SkillDict[currentSkillCode].targets,false);
        }
    }

    void SelectTargets(Skill.SKILL_TARGETS targetType, bool turnOffHighlights)
    {
        foreach (EntityBase entity in listOfTargets)
            entity.outline.eraseRenderer = true;

        switch (targetType)
        {
            case Skill.SKILL_TARGETS.SINGLE_TARGET:
                listOfTargets[currentTargetNum].outline.eraseRenderer = turnOffHighlights;
                break;

            case Skill.SKILL_TARGETS.ADJACENT:
                listOfTargets[currentTargetNum].outline.eraseRenderer = turnOffHighlights;

                if (currentTargetNum - 1 >= 0)
                    listOfTargets[currentTargetNum - 1].outline.eraseRenderer = turnOffHighlights;
                if (currentTargetNum + 1 < listOfTargets.Count)
                    listOfTargets[currentTargetNum + 1].outline.eraseRenderer = turnOffHighlights;
                break;

            case Skill.SKILL_TARGETS.ALL:
                foreach (EntityBase entity in listOfTargets)
                    entity.GetComponent<Enemy>().outline.eraseRenderer = turnOffHighlights;
                break;

            case Skill.SKILL_TARGETS.NONE:
                break;
        }
    }

    void UseSkill(SKILL_CODE skill)
    {
        attacking = true;
        Attack(skillSet.SkillDict[skill]);
    }

    void Attack(Skill skill)
    {
        skill.Use(this, listOfTargets[currentTargetNum]);
        SelectTargets(skill.targets, true);
        currentTargetNum = 0;
        keyIndex = (int)SKILL_CODE.NONE;
        listOfTargets.Clear();
        attacking = false;
    }
}