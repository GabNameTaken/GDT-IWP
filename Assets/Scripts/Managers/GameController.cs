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
        BlessingManager.Instance.StartSelection();
    }
    public void BeginBattle()
    {
        CombatManager.Instance.StartBattle(currentZone);
    }

    public void Victory(bool win)
    {
        if (win)
        {
            UIManager.Instance.SetWinScreenActive(true);
        }
        else
        {
            UIManager.Instance.SetLoseScreenActive(true);
        }
    }

}
