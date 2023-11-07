using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayableCharacter : EntityBase
{
    [SerializeField] SkillSet skillSet;
    bool selectedS1, selectedS2, selectedS3 = false;

    public int currentTargetNum = 0;
    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
    }

    private void Update()
    {
        if (!isMoving) return;

        SkillSelectionCheck();
        SelectTarget();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            if (selectedS1)
            {
                selectedS1 = false;

                MoveToTarget(skillSet.S1);
                currentTargetNum = 0;
            }
        }
    }

    void MoveToTarget(Skill skill)
    {
        Tween moveTween = transform.DOJump(listOfTargets[currentTargetNum].transform.position, 0.5f, 1, 0.5f);
        moveTween.OnComplete(() => Attack(skill));
    }

    void Attack(Skill skill)
    {
        skill.Use(this, listOfTargets[currentTargetNum]);
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
}