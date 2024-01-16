using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lasting Effect", menuName = "Augment/Effect/Lasting Effect/Team HP Boost Effect")]
public class TeamHPBoostEffect : LastingEffect
{
    [SerializeField] float HPBoostPercentage;
    public override void RegisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.maxHealth *= (1 + HPBoostPercentage/100f);
            Debug.Log(teammate.trueStats.maxHealth);
        }
    }

    public override void UnregisterEffect()
    {
        foreach (PlayableCharacter teammate in CombatManager.Instance.PlayerParty)
        {
            teammate.trueStats.maxHealth /= (1 + HPBoostPercentage / 100f);
            Debug.Log(teammate.trueStats.maxHealth);
        }
    }
}
