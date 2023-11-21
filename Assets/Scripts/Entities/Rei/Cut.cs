using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Skills/Cut")]
public class Cut : Skill
{
    public override void Use(EntityBase attacker, EntityBase attackee)
    {
        attacker.originalPosition = attacker.transform.position;
        attacker.originalRotation = attacker.transform.rotation;

        CameraManager.Instance.MoveCamera(attackee.gameObject, CAMERA_POSITIONS.HIGH_FRONT_SELF, 0.1f);

        Tween moveTween = attacker.transform.DOJump(attackee.transform.position, 0.5f, 1, 0.5f);
        moveTween.OnComplete(() =>
        {
            attacker.animator.Play("CutAttack");
            base.Use(attacker, attackee);
        });
    }
}
