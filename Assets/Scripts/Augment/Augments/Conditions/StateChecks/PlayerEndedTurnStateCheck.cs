using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "State Check/Player Ended Turn State Check")]
public class PlayerEndedTurnStateCheck : StateCheck
{
    // Not mentioned but it'll only return true if the entity taking turn is an ally
    public override bool CheckState()
    {
        return CombatManager.Instance.PlayerParty.Any(a => a is EntityBase entityBase && entityBase == CombatManager.Instance.GetEntityEndedTurn());
    }
}
