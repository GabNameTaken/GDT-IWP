using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Night Night")]
public class NightNight : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.animator.Play("NightNightAttack");

        CameraManager.Instance.MoveCamera(attackee.gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        base.Use(attacker, attackee);
    }
}
