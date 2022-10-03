using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance = null;


    public bool gameOver = false;
    private bool isRunning;

    private SceneController.Level currentLevel;

    void Start() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Start with main game level
        SceneController.LoadLevel( SceneController.Level.MAIN_LEVEL );
    }

    void Update() {
        switch( SceneController.GetCurrentLevel() ) {
            case SceneController.Level.MAIN_LEVEL:
                RunGame();
                break;
            case SceneController.Level.GAME_OVER_MENU:
            case SceneController.Level.WIN_MENU:
                RunGameEnd();
                break;
        }
    }

    void RunGame() {}

    void RunGameEnd()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) ) {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Reset game
        PlayController.instance.Restart();
        SceneController.LoadLevel( SceneController.Level.MAIN_LEVEL );

        // Unfreeze time
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        SceneController.LoadLevel(SceneController.Level.GAME_OVER_MENU);

        Time.timeScale = 0.0f;
    }

    public void GameWon()
    {
        SceneController.LoadLevel(SceneController.Level.WIN_MENU);

        Time.timeScale = 0.0f;
    }
}