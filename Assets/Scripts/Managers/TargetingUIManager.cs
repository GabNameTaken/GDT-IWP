using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;

public class TargetingUIManager : Singleton<TargetingUIManager>
{
    List<GameObject> targetUIList = new List<GameObject>();
    [SerializeField] GameObject enemyTargetPrefab, allyTargetPrefab;

    [SerializeField] float secondaryTargetScale, yScaleOffset, minCameraDelay;
    [SerializeField] Transform targetParent;

    Coroutine registerTargetsCoroutine;

    public void RegisterTargets(Skill.SKILL_TARGET_TEAM targetTeam, List<EntityBase> mainTargets, List<EntityBase> secondaryTargets, float delay)
    {
        ClearTargets();
        if (registerTargetsCoroutine != null) StopCoroutine(registerTargetsCoroutine);

        if (delay < minCameraDelay) delay = minCameraDelay;
        registerTargetsCoroutine = StartCoroutine(RegisterTargetsCoroutine(targetTeam, mainTargets, secondaryTargets, delay));
    }

    private IEnumerator RegisterTargetsCoroutine(Skill.SKILL_TARGET_TEAM targetTeam, List<EntityBase> mainTargets, List<EntityBase> secondaryTargets, float delay)
    {
        yield return StartCoroutine(Delay(delay));

        if (secondaryTargets == null)
            secondaryTargets = new List<EntityBase>(); // If no secondary targets, initialize an empty list for it

        List<EntityBase> targets = mainTargets;
        targets.AddRange(secondaryTargets);

        foreach (EntityBase target in targets)
        {
            GameObject newTargetUI = Instantiate(targetTeam == Skill.SKILL_TARGET_TEAM.ENEMY ? enemyTargetPrefab : allyTargetPrefab, targetParent);
            RectTransform targetRectTransform = newTargetUI.GetComponent<RectTransform>();

            targetRectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(target.GetComponent<EntityBase>().model.transform.position);
            targetRectTransform.anchoredPosition = new Vector2(targetRectTransform.anchoredPosition.x, targetRectTransform.anchoredPosition.y + targetRectTransform.rect.height * yScaleOffset);

            if (secondaryTargets.Contains(target))
                targetRectTransform.localScale = new Vector2(targetRectTransform.localScale.x * secondaryTargetScale, targetRectTransform.localScale.y * secondaryTargetScale);

            targetUIList.Add(newTargetUI);
        }
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void ClearTargets()
    {
        foreach (GameObject targetUI in targetUIList) if (targetUI) Destroy(targetUI);
    }

    public void SkillUsed()
    {
        ClearTargets();
    }
}
