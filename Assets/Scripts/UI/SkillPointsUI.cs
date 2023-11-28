using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillPointsUI : MonoBehaviour
{
    List<GameObject> skillPoints = new();
    [SerializeField] Color consumeColor;
    private void Start()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            skillPoints.Add(childObject);
        }
    }

    public void AddSkillPoint(int num)
    {
        List<GameObject> selectedSkillPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
        for (int i = 0; i < -num; i++)
        {
            if (selectedSkillPoints[i])
                selectedSkillPoints[i].GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
        }
    }

    public void ConsumeSkillPoints(int num)
    {
        for (int i = 1; i <= num; i++)
        {
            if (skillPoints[PlayerTeamManager.Instance.skillPoints - i])
                skillPoints[PlayerTeamManager.Instance.skillPoints - i].GetComponent<Image>().color = consumeColor;
        }
    }

    List<Tween> loopTweens = new();
    public void SelectingSkillPoints(int num)
    {
        if (num > 0)
        {
            for (int i = 1; i <= num; i++)
            {
                if (skillPoints[PlayerTeamManager.Instance.skillPoints - i])
                {
                    Tween loop = skillPoints[PlayerTeamManager.Instance.skillPoints - i].GetComponent<Image>().DOColor(consumeColor, 1f).SetLoops(-1, LoopType.Yoyo);
                    loopTweens.Add(loop);
                }
            }
        }
        else
        {
            List<GameObject> selectedSkillPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
            if (selectedSkillPoints.Count() > 0)
            {
                for (int i = 0; i < -num; i++)
                {
                    if (selectedSkillPoints[i])
                    {
                        Tween loop = selectedSkillPoints[i].GetComponent<Image>().DOColor(Color.white, 1f).SetLoops(-1, LoopType.Yoyo);
                        loopTweens.Add(loop);
                    }
                }
            }
        }
    }

    public void KillLoops()
    {
        foreach (Tween loop in loopTweens)
        {
            loop.Restart();
            loop.Kill();
        }
        loopTweens.Clear();
    }
}
