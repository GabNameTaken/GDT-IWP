using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lasting Effect", menuName = "Augment/Effect/Lasting Effect/Team ATK Boost Effect")]
public class TeamATKBoostEffect : LastingEffect
{
    [SerializeField] float flatATKBoost;
    public override void RegisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.attack += flatATKBoost;
            Debug.Log(teammate.trueStats.attack);
        }
    }

    public override void UnregisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.attack -= flatATKBoost;
            Debug.Log(teammate.trueStats.attack);
        }
    }
}
