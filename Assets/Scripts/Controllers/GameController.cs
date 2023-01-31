using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance = null;

    [SerializeField]
    private bool gameOver = false;

    [SerializeField]
    private bool pauseGame = false;

    private SceneController.Level currentLevel;

    void Start() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Start with main menu
        SceneController.LoadLevel( SceneController.Level.MAIN_MENU );
    }

    void Update() {
        switch( SceneController.GetCurrentLevel() ) {
            case SceneController.Level.MAIN_MENU:
                RunMenu();
                break;
            case SceneController.Level.PAUSE_MENU:
                break;
            case SceneController.Level.MAIN_LEVEL:
                RunGame();
                break;
            case SceneController.Level.GAME_OVER_MENU:
            case SceneController.Level.WIN_MENU:
                RunGameEnd();
                break;
        }
    }

    void RunMenu()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) ) {
            SceneController.LoadLevel( SceneController.Level.MAIN_LEVEL );
        }

        Time.timeScale = 0.0f;
    }

    void RunGame()
    {
        Time.timeScale = 1.0f;
    }

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

    public void GameOver(int score)
    {
        SceneController.LoadLevel(SceneController.Level.GAME_OVER_MENU);
        UIController.uicInstance.SetFinalScore(score);

        Time.timeScale = 0.0f;
    }

    public void GameWon()
    {
        SceneController.LoadLevel(SceneController.Level.WIN_MENU);

        Time.timeScale = 0.0f;
    }

    public bool IsGameOver()
    {
        bool isGameOver = gameOver;

        SceneController.Level currLevel = SceneController.GetCurrentLevel();
        isGameOver = isGameOver || (currLevel == SceneController.Level.GAME_OVER_MENU);
        isGameOver = isGameOver || (currLevel == SceneController.Level.WIN_MENU);

        return ( isGameOver );
    }

    public bool IsGamePaused()
    {
        bool isGamePaused = pauseGame;

        SceneController.Level currLevel = SceneController.GetCurrentLevel();
        isGamePaused = isGamePaused || (currLevel == SceneController.Level.PAUSE_MENU);

        return ( isGamePaused );
    }
}