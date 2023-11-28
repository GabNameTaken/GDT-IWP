using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillPointsUI : MonoBehaviour
{
    List<GameObject> skillPoints = new();

    private void Awake()
    {
        foreach (GameObject child in transform)
            skillPoints.Add(child);
    }

    public void AddSkillPoint(int num)
    {
        List<GameObject> selectedSkillPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
        for (int i = 0; i < num; i++)
        {
            selectedSkillPoints[i].GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
        }
    }

    public void ConsumeSkillPoints(int num)
    {
        List<GameObject> selectedSkillPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
        for (int i = 0; i < num; i++)
        {
            selectedSkillPoints[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void ConsumingSkillPoints(int num)
    {
        List<GameObject> selectedSkillPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
        for (int i = 0; i < num; i++)
        {
            selectedSkillPoints[i].GetComponent<Image>().DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void KillTweens()
    {
        foreach (GameObject skillPoint in skillPoints)
        {
            skillPoint.transform.DOKill();
        }
    }
}
