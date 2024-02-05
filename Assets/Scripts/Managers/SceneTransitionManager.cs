using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common.DesignPatterns;

public class SceneTransitionManager : SingletonPersistent<SceneTransitionManager>
{
    public void EnterGame()
    {
        AudioManager.Instance.StopMusic();
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void ToMenu()
    {
        AudioManager.Instance.StopMusic();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
