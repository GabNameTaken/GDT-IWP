using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCharge : MonoBehaviour
{
    [SerializeField] int maxEther = 30;
    int turnCharge;
    int ether;

    [SerializeField] Slider slider;

    bool isSelectingTurn = false;
    public bool turnStolen = false;

    List<PlayableCharacter> playerList = new();
    private void Awake()
    {
        slider.maxValue = maxEther;
    }

    void ConsumeCharge(int charge)
    {
        if (ether < 10)
            return;
        ConsumeEther(charge);

        //take turn
        CombatManager.Instance.stealTurn = true;
        isSelectingTurn = true;
        playerList.Clear();
        foreach (PlayableCharacter survivor in CombatManager.Instance.playerParty)
        {
            playerList.Add(survivor);
        }
        CameraManager.Instance.MoveCamera(MapManager.Instance.currentMap.transform.Find("CombatSetup").gameObject, CAMERA_POSITIONS.PLAYER_TEAM_FRONT, 0.5f);
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

    int currentTargetNum = 0;
    void SelectTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            
            if (currentTargetNum < playerList.Count - 1)
                currentTargetNum++;
            SelectTarget(playerList[currentTargetNum], false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentTargetNum > 0)
                currentTargetNum--;
            SelectTarget(playerList[currentTargetNum], false);
        }
    }

    void SelectTarget(PlayableCharacter selected, bool turnOffHighlights)
    {
        foreach (EntityBase entity in playerList)
            entity.outline.eraseRenderer = true;
        if (turnOffHighlights)
            return;

        selected.outline.eraseRenderer = turnOffHighlights;
    }

    private void Update()
    {
        if (CombatManager.Instance.isPlayerTurn)
            return;
        if (!isSelectingTurn && Input.GetKeyDown(KeyCode.Space))
        {
            ConsumeCharge(10);
        }
        if (turnStolen && isSelectingTurn)
        {
            SelectTargetInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CombatManager.Instance.StealNextTurn(playerList[currentTargetNum]);
                playerList[currentTargetNum].outline.eraseRenderer = true;
                isSelectingTurn = false;
                CombatManager.Instance.isPlayerTurn = true;
            }
        }
    }
}
