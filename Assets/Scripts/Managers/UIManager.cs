using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject pauseScreen, winScreen, loseScreen;
    public void SetPauseScreenActive(bool active) => pauseScreen.SetActive(active);
    public void SetWinScreenActive(bool active) => winScreen.SetActive(active);
    public void SetLoseScreenActive(bool active) => loseScreen.SetActive(active);
}
