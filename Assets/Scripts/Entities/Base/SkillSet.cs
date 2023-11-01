using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSet
{
    public Skill S1, S2, S3, PassiveSkill;

    public SkillSet(SkillSet set)
    {
        S1 = set.S1;
        S2 = set.S2;
        S3 = set.S3;
        PassiveSkill = set.PassiveSkill;
    }
}
