using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PlayableCharacter : EntityBase
{
    public int currentTargetNum = 0;
    List<EntityBase> targets = new();
    override protected void Awake()
    {
        _skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);

        for (int i = 0; i < skillSet.SkillDict.Count; i++)
        {
            skillSet.SkillDict[(SKILL_CODE)i].currentCooldown = 0;
        }
        base.Awake();
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
            if (Input.GetKeyDown(skillKeys[i]) && skillSet.SkillDict[(SKILL_CODE)i].currentCooldown <= 0)
            {
                keyIndex = i;
                CombatUIManager.Instance.DisplaySelectedSkill((SKILL_CODE)keyIndex);

                if (currentSkillCode == (SKILL_CODE)keyIndex)
                    UseSkill(currentSkillCode);

                if ((SKILL_CODE)keyIndex != SKILL_CODE.NONE)
                {
                    SelectTargets(skillSet.SkillDict[(SKILL_CODE)keyIndex].targetTeam, skillSet.SkillDict[(SKILL_CODE)keyIndex].targets, false);
                    skillSet.SkillDict[(SKILL_CODE)keyIndex].PlayReadyAnimation(this);
                    PlayerTeamManager.Instance.StopHover();
                    PlayerTeamManager.Instance.UpdateSkillPoints(skillSet.SkillDict[(SKILL_CODE)keyIndex].skillCost, false);
                }
            }
            else if (Input.GetKeyDown(skillKeys[i]) && skillSet.SkillDict[(SKILL_CODE)i].currentCooldown > 0) PopupMessage.Instance.PopMessage("Skill on cooldown");
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

    bool invertTargetting = false;
    void SelectTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (invertTargetting)
            {
                if (currentTargetNum < listOfTargets.Count - 1)
                    currentTargetNum++;
            }
            else if (currentTargetNum > 0)
                currentTargetNum--;
            if (currentSkillCode != SKILL_CODE.NONE)
                SelectTargets(skillSet.SkillDict[currentSkillCode].targetTeam, skillSet.SkillDict[currentSkillCode].targets,false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (invertTargetting)
            {
                if (currentTargetNum > 0)
                    currentTargetNum--;
            }
            else if (currentTargetNum < listOfTargets.Count - 1)
                currentTargetNum++;
            if (currentSkillCode != SKILL_CODE.NONE)
                SelectTargets(skillSet.SkillDict[currentSkillCode].targetTeam, skillSet.SkillDict[currentSkillCode].targets,false);
        }
    }

    void RotateToTarget()
    {
        // Calculate the direction vector towards the enemy
        Vector3 directionToEnemy = (listOfTargets[currentTargetNum].transform.position - transform.position).normalized;

        // Calculate the rotation quaternion to look in that direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy, Vector3.up);

        transform.DORotateQuaternion(targetRotation, 0.2f);
    }

    void SelectTargets(Skill.SKILL_TARGET_TEAM targettedTeam, Skill.SKILL_TARGETS targetType, bool turnOffHighlights)
    {
        List<EntityBase> primaryTargets = new List<EntityBase>(), secondaryTargets = new List<EntityBase>();
        foreach (EntityBase entity in listOfTargets)
            entity.outline.eraseRenderer = true;
        if (turnOffHighlights)
            return;

        List<Enemy> enemies = CombatManager.Instance.EnemyParty.Where(enemy => !enemy.IsDead).ToList();
        List<PlayableCharacter> allies = CombatManager.Instance.PlayerParty.Where(player => !player.IsDead).ToList();

        listOfTargets.Clear();
        invertTargetting = false;
        if (targettedTeam == Skill.SKILL_TARGET_TEAM.ALLY)
        {
            listOfTargets.AddRange(allies);
            CameraManager.Instance.MoveCamera(MapManager.Instance.battleground, CAMERA_POSITIONS.PLAYER_TEAM_FRONT, 0.5f);
            invertTargetting = true;
        }
        else if (targettedTeam == Skill.SKILL_TARGET_TEAM.ENEMY)
        {
            listOfTargets.AddRange(enemies);
            CameraManager.Instance.MoveCamera(gameObject, CAMERA_POSITIONS.LOW_BACK, 0f);
        }
        else if (targettedTeam == Skill.SKILL_TARGET_TEAM.SELF)
        { 
            listOfTargets.Add(this);
            CameraManager.Instance.MoveCamera(gameObject, CAMERA_POSITIONS.LOW_FRONT_SELF, 0f);
        }

        targets.Clear();
        switch (targetType)
        {
            case Skill.SKILL_TARGETS.SINGLE_TARGET:
                if (currentTargetNum >= listOfTargets.Count)
                    currentTargetNum = 0;
                listOfTargets[currentTargetNum].outline.eraseRenderer = turnOffHighlights;
                targets.Add(listOfTargets[currentTargetNum]);
                primaryTargets.Add(listOfTargets[currentTargetNum]);
                if (targettedTeam == Skill.SKILL_TARGET_TEAM.ENEMY)
                    RotateToTarget();
                break;

            case Skill.SKILL_TARGETS.ADJACENT:
                if (currentTargetNum >= listOfTargets.Count)
                    currentTargetNum = 0;
                listOfTargets[currentTargetNum].outline.eraseRenderer = turnOffHighlights;
                targets.Add(listOfTargets[currentTargetNum]);
                primaryTargets.Add(listOfTargets[currentTargetNum]);
                if (targettedTeam == Skill.SKILL_TARGET_TEAM.ENEMY)
                    RotateToTarget();

                if (currentTargetNum - 1 >= 0)
                {
                    listOfTargets[currentTargetNum - 1].outline.eraseRenderer = turnOffHighlights;
                    targets.Add(listOfTargets[currentTargetNum - 1]);
                    secondaryTargets.Add(listOfTargets[currentTargetNum - 1]);
                }
                if (currentTargetNum + 1 < listOfTargets.Count)
                {
                    listOfTargets[currentTargetNum + 1].outline.eraseRenderer = turnOffHighlights;
                    targets.Add(listOfTargets[currentTargetNum + 1]);
                    secondaryTargets.Add(listOfTargets[currentTargetNum + 1]);
                }
                break;

            case Skill.SKILL_TARGETS.ALL:
                foreach (EntityBase entity in listOfTargets)
                    entity.outline.eraseRenderer = turnOffHighlights;
                targets.AddRange(listOfTargets);
                primaryTargets.AddRange(listOfTargets);
                break;

            case Skill.SKILL_TARGETS.NONE:
                break;
        }
        TargetingUIManager.Instance.RegisterTargets(targettedTeam, primaryTargets, secondaryTargets, CameraManager.Instance.cameraDelay);
    }

    void UseSkill(SKILL_CODE skill)
    {
        if (PlayerTeamManager.Instance.skillPoints < skillSet.SkillDict[skill].skillCost)
        {
            PopupMessage.Instance.PopMessage("Insufficient skill points");
            Debug.Log("Insufficient skill points");
            return;
        }
        if (skillSet.SkillDict[skill].currentCooldown > 0)
        {
            PopupMessage.Instance.PopMessage("Skill on cooldown");
            Debug.Log("Skill on cooldown");
            return;
        }

        attacking = true;
        Attack(skillSet.SkillDict[skill]);
        TargetingUIManager.Instance.SkillUsed();
    }

    public int etherCharge = 1;

    protected override void Attack(Skill skill)
    {
        skill.Use(this, targets);
        SelectTargets(skill.targetTeam, skill.targets, true);
        CombatManager.Instance.turnCharge.AddEther(etherCharge);

        PlayerTeamManager.Instance.StopHover();
        currentTargetNum = 0;
        keyIndex = (int)SKILL_CODE.NONE;
        listOfTargets.Clear();

        base.Attack(skill);
        CombatUIManager.Instance.DisplaySkillCooldown(skillSet);
        CombatUIManager.Instance.DeselectSkills();
    }
}