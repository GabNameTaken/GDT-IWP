using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "State Check/Class Turn State Check")]
public class ClassTurnStateCheck : StateCheck
{
    [SerializeField] ClassType classType;

    // Not mentioned but it'll only return true if the entity taking turn is an ally
    public override bool CheckState()
    {
        return CombatManager.Instance.PlayerParty.Any(a => a is EntityBase entityBase && entityBase.isMoving && entityBase.entity.classType == this.classType);
    }
}
