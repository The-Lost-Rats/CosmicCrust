using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController gcInstance = null;

    private bool isGameOver = false;

    private bool isPaused = false;

    private SceneController.Level currentLevel;

    void Start() {
        if (gcInstance == null)
        {
            gcInstance = this;
        }
        else if (gcInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Start with main menu
        SceneController.LoadLevel( SceneController.Level.MAIN_MENU );

        // So we start off paused
        PauseGame();
    }

    void Update() {
        SceneController.Level currLevel = SceneController.GetCurrentLevel();
        switch( currLevel )
        {
            case SceneController.Level.MAIN_MENU:
                RunMenu();
                break;
            case SceneController.Level.PAUSE_MENU: // TODO: Future ticket
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

    private void RunMenu()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) )
        {
            SceneController.LoadLevel( SceneController.Level.MAIN_LEVEL );
            UnPauseGame();
        }
    }

    private void RunGame() {}

    private void RunGameEnd()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) )
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Reset game
        PlayController.pcInstance.Restart();
        SceneController.LoadLevel( SceneController.Level.MAIN_LEVEL );
        UnPauseGame();
    }

    public void EndGame(bool wonGame, int score=0)
    {
        isGameOver = true;

        if (wonGame)
        {
            SceneController.LoadLevel(SceneController.Level.WIN_MENU);
        }
        else
        {
            SceneController.LoadLevel(SceneController.Level.GAME_OVER_MENU);
            UIController.uicInstance.SetFinalScore(score);
        }

        PauseGame();
    }

    private void PauseGame()
    {
        if ( !isPaused )
        {
            isPaused = true;
            Time.timeScale = 0.0f;

            // Set default cursor
            InputController.Instance.SetDefaultCursor();
        }
    }

    private void UnPauseGame()
    {
        if ( isPaused )
        {
            isPaused = false;

            // Unfreeze time
            Time.timeScale = 1.0f;
        }
    }

    public bool IsGameOver()
    {
        return ( isGameOver );
    }

    public bool IsGamePaused()
    {
        return ( isPaused );
    }
}