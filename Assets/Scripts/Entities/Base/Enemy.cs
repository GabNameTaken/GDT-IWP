using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : EntityBase
{
    [SerializeField] SkillSet skillSet;
    private void Awake()
    {
        skillSet = new SkillSet(entity.baseSkillSet);
        trueStats = new Stats(entity.baseStats.Stats);
    }

    public void SetToMove()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        PlayableCharacter target = SelectSingleTarget();
        Tween jumpOnToPlayer = transform.DOJump(target.transform.position, 0.5f, 1, 1);
        jumpOnToPlayer.OnComplete(SelectSkill);
    }

    void SelectSkill()
    {
        skillSet.S1.Use(this, listOfTargets[0]);
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
