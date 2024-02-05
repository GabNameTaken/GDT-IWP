using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public void EnterGame()
    {
        SceneTransitionManager.Instance.EnterGame();
    }

    public void ExitGame()
    {
        SceneTransitionManager.Instance.ExitGame();
    }
}
