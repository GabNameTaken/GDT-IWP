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
            //target
            //use skill
        }
        else if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S2) && skillSet.SkillDict[SKILL_CODE.S2].currentCooldown <= 0)
        {

        }
        else if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S1))
        {
            SelectSingleTarget();
            Attack(skillSet.SkillDict[SKILL_CODE.S1]);
        }
    }

    PlayableCharacter SelectSingleTarget()
    {
        List<PlayableCharacter> listOfTargetable = new();
        listOfTargets.Clear();
        foreach (PlayableCharacter playableCharacter in CombatManager.Instance.PlayerParty)
        {
            if (!playableCharacter.IsDead)
                listOfTargetable.Add(playableCharacter);
        }

        // Check if there are valid targets in the list
        if (listOfTargetable.Count > 0)
        {
            // Generate a random index within the range of valid indices
            int randomIndex = UnityEngine.Random.Range(0, listOfTargetable.Count);

            // Select the random PlayableCharacter
            PlayableCharacter randomTarget = listOfTargetable[randomIndex];

            // Now 'randomTarget' contains the randomly selected PlayableCharacter
            listOfTargets.Add(randomTarget);
            return randomTarget;
        }
        else
        {
            // Handle the case when there are no valid targets
            Debug.Log("No target for enemy");
        }
        return null;
    }

    protected override void Attack(Skill skill)
    {
        skill.Use(this, listOfTargets[0]);
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
