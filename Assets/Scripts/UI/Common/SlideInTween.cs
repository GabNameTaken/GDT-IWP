using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideInTween : MonoBehaviour
{
    enum SLIDE_TYPE
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    [SerializeField] SLIDE_TYPE slideType;
    [SerializeField] float slideDuration = 0.5f;

    RectTransform rectTransform;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        SlideIn();
    }

    void SlideIn()
    {
        // Calculate the target anchored position based on the slide type
        Vector2 targetPosition = Vector2.zero;
        switch (slideType)
        {
            case SLIDE_TYPE.UP:
                targetPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
                break;
            case SLIDE_TYPE.DOWN:
                targetPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
                break;
            case SLIDE_TYPE.LEFT:
                targetPosition = new Vector2(0, rectTransform.anchoredPosition.y);
                break;
            case SLIDE_TYPE.RIGHT:
                targetPosition = new Vector2(0, rectTransform.anchoredPosition.y);
                break;
        }

        // Set the initial position based on the slide type
        Vector2 startPosition = GetStartPosition(targetPosition);

        // Slide in the GameObject
        rectTransform.anchoredPosition = startPosition;
        rectTransform.DOAnchorPos(targetPosition, slideDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(UpdateType.Normal, true);
    }

    Vector2 GetStartPosition(Vector2 targetPosition)
    {
        switch (slideType)
        {
            case SLIDE_TYPE.UP:
                return new Vector2(targetPosition.x, -rectTransform.rect.height);
            case SLIDE_TYPE.DOWN:
                return new Vector2(targetPosition.x, rectTransform.rect.height);
            case SLIDE_TYPE.LEFT:
                return new Vector2(-rectTransform.rect.width, targetPosition.y);
            case SLIDE_TYPE.RIGHT:
                return new Vector2(rectTransform.rect.width, targetPosition.y);
            default:
                return Vector2.zero;
        }
    }
}
