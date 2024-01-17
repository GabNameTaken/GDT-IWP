using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "State Check/Enemy Ended Turn State Check")]
public class EnemyEndedTurnStateCheck : StateCheck
{
    public override bool CheckState()
    {
        return CombatManager.Instance.EnemyParty.Any(a => a is EntityBase entityBase && entityBase == CombatManager.Instance.GetEntityEndedTurn());
    }
}
