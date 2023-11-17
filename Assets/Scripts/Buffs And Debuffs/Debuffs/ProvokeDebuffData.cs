using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Provoke")]
public class ProvokeDebuffData : DebuffData
{
    public override void ApplyEffect(EntityBase source, EntityBase dest)
    {
        dest.Provoked(source);
    }
}
