using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Enemy : EntityBase
{
    [SerializeField] SkillSet skillSet;
    [SerializeField] Canvas worldSpaceCanvas;

    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
        worldSpaceCanvas.worldCamera = Camera.main;
        if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S2))
            skillSet.SkillDict[SKILL_CODE.S2].currentCooldown = skillSet.SkillDict[SKILL_CODE.S2].cooldown;
        if (skillSet.SkillDict.ContainsKey(SKILL_CODE.S3))
            skillSet.SkillDict[SKILL_CODE.S3].currentCooldown = skillSet.SkillDict[SKILL_CODE.S3].cooldown;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();
        SelectSkill();
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
            PlayableCharacter target = SelectSingleTarget();
            skillSet.SkillDict[SKILL_CODE.S1].Use(this, target);
        }
    }

    PlayableCharacter SelectSingleTarget()
    {
        List<PlayableCharacter> listOfTargetable = new();
        listOfTargets.Clear();
        foreach (PlayableCharacter playableCharacter in CombatManager.Instance.playerParty)
        {
            if (!playableCharacter.isDead)
                listOfTargetable.Add(playableCharacter);
        }

        // Check if there are valid targets in the list
        if (listOfTargetable.Count > 0)
        {
            // Generate a random index within the range of valid indices
            int randomIndex = UnityEngine.Random.Range(0, listOfTargets.Count);

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
}
