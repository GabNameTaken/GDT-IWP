using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
using UnityEngine.UI;

public class BattleSpeedManager : Singleton<BattleSpeedManager>
{
    BATTLE_SPEED battleSpeed = BATTLE_SPEED.SPEED_1X;
    bool paused;

    [SerializeField] GameObject pauseBG;

    public void SetBattleSpeed()
    {
        Time.timeScale = paused ? 0f : BattleSpeedToTimeScale(battleSpeed);
    }

    public void CycleBattleSpeed()
    {
        battleSpeed++;
        if (battleSpeed >= BATTLE_SPEED.BATTLE_SPEED_COUNT) battleSpeed = 0;

        SetBattleSpeed();
    }

    public void TogglePause()
    {
        paused = !paused;

        pauseBG.SetActive(paused);
        SetBattleSpeed();
    }

    private float BattleSpeedToTimeScale(BATTLE_SPEED battleSpeed)
    {
        switch (battleSpeed)
        {
            case BATTLE_SPEED.SPEED_1X:
                return 1f;
            case BATTLE_SPEED.SPEED_2X:
                return 2f;
            default:
                return 1f;
        }
    }
}

public enum BATTLE_SPEED
{
    SPEED_1X,
    SPEED_2X,
    BATTLE_SPEED_COUNT
}
