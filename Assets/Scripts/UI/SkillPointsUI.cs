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

    List<Tween> loopTweens = new();
    public void AddSkillPoint(int num, bool add)
    {
        List<GameObject> selectedPoints = new();
        List<GameObject> currentPoints = skillPoints.Skip(PlayerTeamManager.Instance.skillPoints).ToList();
        if (currentPoints.Count() > 0)
        {
            for (int i = 0; i < -num; i++)
            {
                if (currentPoints[i])
                {
                    selectedPoints.Add(currentPoints[i]);
                }
            }
        }

        foreach (GameObject point in selectedPoints)
        {
            Tween loop = point.GetComponent<Image>().DOColor(Color.white, 1f).SetLoops(-1, LoopType.Yoyo);
            loopTweens.Add(loop);
        }

        if (add)
        {
            KillLoops();
            foreach (GameObject point in selectedPoints)
            {
                point.GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
            }
        }
    }

    public void ConsumeSkillPoints(int num, bool consume)
    {
        List<GameObject> selectedPoints = new();
        for (int i = 1; i <= num; i++)
        {
            if (skillPoints[PlayerTeamManager.Instance.skillPoints - i])
            {
                selectedPoints.Add(skillPoints[PlayerTeamManager.Instance.skillPoints - i]);
            }
        }

        foreach (GameObject point in selectedPoints)
        {
            Tween loop = point.GetComponent<Image>().DOColor(consumeColor, 1f).SetLoops(-1, LoopType.Yoyo);
            loopTweens.Add(loop);
        }

        if (consume)
        {
            KillLoops();
            foreach (GameObject point in selectedPoints)
            {
                point.GetComponent<Image>().color = consumeColor;
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
