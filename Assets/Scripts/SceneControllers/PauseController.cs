using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : ISceneController
{
    override protected GameState GetGameState() { return GameState.PAUSE; }

    void Start()
    {
        Time.timeScale = 0.0f;
    }

    override protected void SceneUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 1.0f;
            GameController.instance.ChangeState(GameState.PLAY);
        }
    }
}
