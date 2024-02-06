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
        MapManager.Instance.currentMapNum = -1;
        MapManager.Instance.NextMap();
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
            AudioManager.Instance.StopMusic();
        }
    }

    public void GameCleared()
    {
        UIManager.Instance.SetGameCompletedScreenActive(true);
    }

    public void ToMainMenu()
    {
        SceneTransitionManager.Instance.ToMenu();
    }
}
