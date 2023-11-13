using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuff/Provoke")]
public class Provoke : Debuff
{
    public EntityBase debuffer;
    public override void Apply(EntityBase entity)
    {
        if (debuffer && !debuffer.isDead)
        {
            entity.listOfTargets.Clear();
            entity.listOfTargets.Add(debuffer);
        }
        else
            Destroy(this);

        base.Apply(entity);
    }
}
