using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : EntityBase
{
    [SerializeField] SkillSet skillSet;
    [SerializeField] Canvas worldSpaceCanvas;
    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
        worldSpaceCanvas.worldCamera = Camera.main;
        if (skillSet.S3)
            skillSet.S3.currentCooldown = skillSet.S3.cooldown;
        if (skillSet.S2)
            skillSet.S2.currentCooldown = skillSet.S2.cooldown;
    }

    public void SetToMove()
    {
        SelectSkill();
    }

    void SelectSkill()
    {
        if (skillSet.S3 && skillSet.S3.currentCooldown <= 0)
        {
            //target
            //use skill
        }
        else if (skillSet.S2 && skillSet.S2.currentCooldown <= 0)
        {

        }
        else if (skillSet.S1)
        {
            PlayableCharacter target = SelectSingleTarget();
            skillSet.S1.Use(this, target);
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
