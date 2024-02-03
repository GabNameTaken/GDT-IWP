using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;

public class GameController : Singleton<GameController>
{
    CombatZone currentZone;
    public void CombatSetup(CombatZone zone)
    {
        currentZone = zone;
        AugmentCardManager.Instance.StartSelection();
    }
    public void BeginBattle()
    {
        CombatManager.Instance.StartBattle(currentZone);
    }

    public void Restart()
    {
        AugmentManager.Instance.ClearAugments();
        MapManager.Instance.currentMapNum = 0;
        MapManager.Instance.SetMap();
    }

    public void Victory(bool win)
    {
        if (win)
        {
            UIManager.Instance.SetBattleWinScreenActive(true);
        }
        else
        {
            UIManager.Instance.SetLoseScreenActive(true);
        }
    }

    public void GameCleared()
    {
        UIManager.Instance.SetGameCompletedScreenActive(true);
    }
}
