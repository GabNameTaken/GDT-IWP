using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject pauseScreen, winScreen, loseScreen, combatUIPage;
    [SerializeField] Button toggleBattleSpeedBtn;
    [SerializeField] Sprite battleSpeed1xImg, battleSpeed2xImg;

    public void SetPauseScreenActive(bool active) => pauseScreen.SetActive(active);
    public void SetWinScreenActive(bool active) 
    {
        winScreen.SetActive(active);
        combatUIPage.SetActive(active);
    }
    public void SetLoseScreenActive(bool active) 
    {
        loseScreen.SetActive(active);
        combatUIPage.SetActive(active);
    }

    public void SetBattleSpeedBtnSprite(BATTLE_SPEED battleSpeed)
    {
        switch (battleSpeed)
        {
            case BATTLE_SPEED.SPEED_1X:
                toggleBattleSpeedBtn.image.sprite = battleSpeed1xImg;
                break;
            case BATTLE_SPEED.SPEED_2X:
                toggleBattleSpeedBtn.image.sprite = battleSpeed2xImg;
                break;
            default:
                break;
        }
    }
}
