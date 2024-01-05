using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Erupt")]
public class Erupt : Skill
{
    public override void Use(EntityBase attacker, List<EntityBase> attackeeList)
    {
        attacker.animator.Play("EruptAttack");
        base.Use(attacker, attackeeList);
    }

    protected override IEnumerator SkillAnimationCoroutine(EntityBase attacker, List<EntityBase> attackeeList)
    {
        base.SkillAnimationCoroutine(attacker, attackeeList);

        yield return new WaitForSeconds(attacker.animator.GetCurrentAnimatorStateInfo(0).length * 0.3f);

        CameraManager.Instance.MoveCamera(attackeeList[0].gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        SkillParticle particle = Instantiate(skillParticle, attackeeList[0].transform);
        particle.ManualPlay(2);

        yield return base.SkillAnimationCoroutine(attacker, attackeeList);
    }
}
