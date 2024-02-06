using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMainMenuBGM();
    }
    public void EnterGame()
    {
        SceneTransitionManager.Instance.EnterGame();
    }

    public void ExitGame()
    {
        SceneTransitionManager.Instance.ExitGame();
    }
}
