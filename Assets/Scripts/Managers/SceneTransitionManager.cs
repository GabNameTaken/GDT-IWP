using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common.DesignPatterns;

public class SceneTransitionManager : SingletonPersistent<SceneTransitionManager>
{
    [SerializeField] string gameSceneStr;
    public void EnterGame()
    {
        SceneManager.LoadSceneAsync(gameSceneStr);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
