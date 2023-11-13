using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SkillSet
{
    [SerializeField] List<SkillCodePair> skillList;
    private Dictionary<SKILL_CODE, Skill> skillDict;
    public Dictionary<SKILL_CODE, Skill> SkillDict  {   get { return skillDict ?? (skillDict = skillList.ToDictionary(item => item.skillCode, item => item.skill)); }   }

    public SkillSet(SkillSet set)
    {
        skillList = new List<SkillCodePair>(set.skillList);
        skillDict = skillList.ToDictionary(item => item.skillCode, item => item.skill);
    }
}

[System.Serializable]
public class SkillCodePair
{
    public SKILL_CODE skillCode;
    public Skill skill;
}