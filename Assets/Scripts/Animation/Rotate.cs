using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    [SerializeField] bool onAwake, affectedByTime = true, UI;
    [SerializeField] Vector3 rotation;
    [SerializeField] float rotateDuration;
    [SerializeField] int loops;
    [SerializeField] RotateMode rotateMode;
    [SerializeField] Ease ease;
    RectTransform rectTransform;

    private void Awake()
    {
        if (UI) rectTransform = GetComponent<RectTransform>();
        if (onAwake) StartRotation();
    }

    void StartRotation()
    {
        if (UI) rectTransform.DORotate(rotation, rotateDuration, rotateMode).SetEase(ease).SetLoops(loops, LoopType.Restart).SetUpdate(UpdateType.Normal, !affectedByTime);
        else transform.DORotate(rotation, rotateDuration, rotateMode).SetEase(ease).SetLoops(loops, LoopType.Restart).SetUpdate(UpdateType.Normal, !affectedByTime);
    }
}
