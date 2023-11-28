using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamManager : MonoBehaviour
{
    public static PlayerTeamManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<GameObject> teamPrefabs;

    //To do reaaranging function for team setup

    [SerializeField] SkillPointsUI skillPointsUI;
    private int totalSkillPoints;
    private int _skillPoints;
    public int skillPoints => _skillPoints;
    private void Start()
    {
        totalSkillPoints = skillPointsUI.transform.childCount;
        _skillPoints = totalSkillPoints;
    }

    public void UpdateSkillPoints(int num)
    {
        if (num < 0)
        {
            skillPointsUI.AddSkillPoint(num);

            if (_skillPoints - num <= totalSkillPoints)
                _skillPoints -= num;
            else
                _skillPoints = totalSkillPoints;
        }
        else
        {
            skillPointsUI.ConsumeSkillPoints(num);

            if (_skillPoints - num > 0)
                _skillPoints -= num;
            else
                _skillPoints = 0;
        }
    }

    public void HoverSkillPoints(bool hover, int num)
    {
        if (hover && num > 0)
            skillPointsUI.ConsumingSkillPoints(num);
        else
            skillPointsUI.KillLoops();
    }
}
