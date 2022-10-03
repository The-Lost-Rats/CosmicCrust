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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update() {
        switch( SceneController.GetCurrentLevel() ) {
            case SceneController.Level.MAIN_LEVEL:
                RunGame();
                break;
        }
    }

    void RunGame() {
    }

    private void OnSceneUnloaded( Scene unloadedScene ) {
        if ( unloadedScene.name == "GameOverScene" )
        {
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
        SceneManager.LoadScene("GameOverScene", LoadSceneMode.Additive);

        Time.timeScale = 0.0f;
    }
}