using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] string gameSceneStr;
    public void EnterGame()
    {
        SceneManager.LoadScene(gameSceneStr);
    }
}
