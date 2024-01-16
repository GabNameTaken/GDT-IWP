using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lasting Effect", menuName = "Augment/Effect/Lasting Effect/Team SPD Boost Effect")]
public class TeamSPDBoostEffect : LastingEffect
{
    [SerializeField] float flatSPDBoost;
    public override void RegisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.speed += flatSPDBoost;
            Debug.Log(teammate.trueStats.speed);
        }
    }

    public override void UnregisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.speed -= flatSPDBoost;
            Debug.Log(teammate.trueStats.speed);
        }
    }
}
