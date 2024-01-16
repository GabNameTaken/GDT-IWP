using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCondition", menuName = "Augment/Conditions/Skill Condition")]
public class SkillCondition : EventCondition<Skill>
{
    public override void AttachEvent()
    {
        CombatManager.Instance.SkillUsedEvent += OnGameEvent;
    }

    public override void DetachEvent()
    {
        CombatManager.Instance.SkillUsedEvent -= OnGameEvent;
    }

    protected override void OnGameEvent(Skill skill)
    {
        if (!Check()) return;
        Effect.RegisterEffect(skill);
    }
}
