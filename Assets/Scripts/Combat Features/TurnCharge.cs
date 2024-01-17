using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCharge : MonoBehaviour
{
    [SerializeField] int maxEther = 30;
    int turnCharge;
    public int ether { get; private set; }

    [SerializeField] Slider slider;

    bool isSelectingTurn = false;

    List<PlayableCharacter> playerList = new();
    private void Awake()
    {
        slider.maxValue = maxEther;
    }

    void ConsumeCharge(int charge)
    {
        if (ether < 10 || isSelectingTurn)
            return;

        ConsumeEther(charge);

        //take turn
        CombatManager.Instance.stealTurn = true;
       
    }

    public void AddEther(int amount)
    {
        ether += amount;
        if (ether > maxEther)
            ether = maxEther;
        turnCharge = ether / 10;
        slider.value = ether;
    }

    public void ConsumeEther(int amount)
    {
        ether -= amount;
        if (ether < 0)
            ether = 0;
        turnCharge = ether / 10;
        slider.value = ether;
    }

    public void SelectTurn()
    {
        isSelectingTurn = true;
        playerList.Clear();
        foreach (PlayableCharacter survivor in CombatManager.Instance.PlayerParty)
        {
            if (!survivor.IsDead)
                playerList.Add(survivor);
        }
        CameraManager.Instance.MoveCamera(MapManager.Instance.currentMap.transform.Find("CombatSetup").gameObject, CAMERA_POSITIONS.PLAYER_TEAM_FRONT, 0.5f);
        SelectTarget(playerList[currentTargetNum], 0.5f);
    }

    int currentTargetNum = 0;
    void SelectTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentTargetNum < playerList.Count - 1)
                currentTargetNum++;
            SelectTarget(playerList[currentTargetNum], 0f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentTargetNum > 0)
                currentTargetNum--;
            SelectTarget(playerList[currentTargetNum], 0f);
        }
    }

    void SelectTarget(PlayableCharacter selected, float delay)
    {
        List<EntityBase> targets = new();
        targets.Add(selected);
        TargetingUIManager.Instance.RegisterTargets(Skill.SKILL_TARGET_TEAM.ALLY, targets, null, delay);
    }

    private void Update()
    {
        //if (CombatManager.Instance.isPlayerTurn)
        //    return;
        if (!isSelectingTurn && Input.GetKeyDown(KeyCode.Tab))
        {
            ConsumeCharge(10);
        }
        if (isSelectingTurn)
        {
            SelectTargetInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CombatManager.Instance.StealNextTurn(playerList[currentTargetNum]);
                isSelectingTurn = false;
                CombatManager.Instance.isPlayerTurn = true;
            }
        }
    }
}
