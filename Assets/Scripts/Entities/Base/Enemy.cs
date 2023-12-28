using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class Enemy : EntityBase
{
    [SerializeField] Canvas worldSpaceCanvas;
    [SerializeField] Image lastHitElementUI;
    private Element lastHitElement;

    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
        worldSpaceCanvas.worldCamera = Camera.main;
        for (int i = 0; i < skillSet.SkillDict.Count; i++)
        {
            skillSet.SkillDict[(SKILL_CODE)i].currentCooldown = skillSet.SkillDict[(SKILL_CODE)i].cooldown;
        }
    }

    public override void TakeTurn()
    {
        CameraManager.Instance.MoveCamera(MapManager.Instance.currentMap.transform.Find("CombatSetup").gameObject, CAMERA_POSITIONS.PLAYER_TEAM_BACK, 1f);
        base.TakeTurn();
    }

    protected override void StartTurn()
    {
        base.StartTurn();
        if (attacking)
            return;
        else
            SelectSkill();
    }

    public override void TakeDamage(float damage, Element element)
    {
        base.TakeDamage(damage, element);
        if (damage > 0) SetLastHitElement(element);
    }

    void SelectSkill()
    {
        if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S3) && skillSet.SkillDict[SKILL_CODE.S3].currentCooldown <= 0)
        {
            Attack(skillSet.SkillDict[SKILL_CODE.S3]);
        }
        else if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S2) && skillSet.SkillDict[SKILL_CODE.S2].currentCooldown <= 0)
        {
            Attack(skillSet.SkillDict[SKILL_CODE.S2]);
        }
        else if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S1))
        {
            Attack(skillSet.SkillDict[SKILL_CODE.S1]);
        }
    }

    void SelectTargetTeam(Skill skill)
    {
        List<EntityBase> targets = new();
        switch(skill.targetTeam)
        {
            case Skill.SKILL_TARGET_TEAM.ENEMY:
                foreach (EntityBase playableCharacter in CombatManager.Instance.PlayerParty)
                {
                    if (!playableCharacter.IsDead)
                        targets.Add(playableCharacter);
                }
                break;
            case Skill.SKILL_TARGET_TEAM.ALLY:
                foreach (EntityBase enemy in CombatManager.Instance.EnemyParty)
                {
                    if (!enemy.IsDead)
                        targets.Add(enemy);
                }
                break;
            case Skill.SKILL_TARGET_TEAM.SELF:
                targets.Add(this);
                break;
        }
        SelectTarget(skill, targets);
    }

    void SelectTarget(Skill skill, List<EntityBase> targets)
    {
        listOfTargets.Clear();
        if (targets.Count < 0)
        {
            Debug.Log("No target for enemy");
            return;
        }
        switch (skill.targets)
        {
            case Skill.SKILL_TARGETS.SINGLE_TARGET:
                listOfTargets.Add(SelectRandomTarget(targets));
                break;
            case Skill.SKILL_TARGETS.ADJACENT:
                EntityBase selectedTarget = SelectRandomTarget(targets);
                int index = targets.IndexOf(selectedTarget);
                if (index - 1 >= 0)
                    listOfTargets.Add(targets[index - 1]);

                listOfTargets.Add(selectedTarget);

                if (index + 1 < targets.Count)
                    listOfTargets.Add(targets[index + 1]);
                break;
            case Skill.SKILL_TARGETS.ALL:
                listOfTargets.AddRange(targets);
                break;
        }
    }

    EntityBase SelectRandomTarget(List<EntityBase> targets)
    {
        // Generate a random index within the range of valid indices
        int randomIndex = UnityEngine.Random.Range(0, targets.Count);

        // Select the random Entity
        EntityBase randomTarget = targets[randomIndex];

        return randomTarget;
    }

    protected override void Attack(Skill skill)
    {
        SelectTargetTeam(skill);
        skill.Use(this, listOfTargets);
        base.Attack(skill);
    }

    private void SetLastHitElement(Element element)
    {
        if (lastHitElement && element && lastHitElement != element)
        {
            PlayerTeamManager.Instance.UpdateSkillPoints(-1, true);
            lastHitElementUI.color = new Color(1, 1, 1, 0);
            lastHitElement = null;
        }
        else if (element)
        {
            lastHitElement = element;
            lastHitElementUI.sprite = element.elementImage;
            lastHitElementUI.color = new Color(1, 1, 1, 1);
        }
    }
}
