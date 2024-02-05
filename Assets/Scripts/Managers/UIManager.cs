using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject pauseScreen, battleWinScreen, loseScreen, gameCompletedScreen, combatUIPage;
    [SerializeField] Button toggleBattleSpeedBtn;
    [SerializeField] Sprite battleSpeed1xImg, battleSpeed2xImg;

    public void SetPauseScreenActive(bool active) => pauseScreen.SetActive(active);
    public void SetBattleWinScreenActive(bool active) 
    {
        battleWinScreen.SetActive(active);
        combatUIPage.SetActive(active);
    }
    public void SetLoseScreenActive(bool active) 
    {
        loseScreen.SetActive(active);
        combatUIPage.SetActive(active);
    }
    public void SetGameCompletedScreenActive(bool active)
    {
        battleWinScreen.SetActive(!active);
        gameCompletedScreen.SetActive(active);
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

    [Header("To Menu Scene")]
    [SerializeField] GameObject menuPrompt;
    [SerializeField] Button menuPromptButton;
    public void DisplayToMenuPrompt(bool toDisplay)
    {
        menuPrompt.SetActive(toDisplay);
        menuPromptButton.gameObject.SetActive(!toDisplay);
    }
}
