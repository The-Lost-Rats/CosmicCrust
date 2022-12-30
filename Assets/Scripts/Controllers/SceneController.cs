using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController scInstance = null;

    public enum Level {
        MAIN_MENU,
        PAUSE_MENU,
        MAIN_LEVEL,
        GAME_OVER_MENU,
        WIN_MENU
    };

    private static Level currLevel;

    public void Awake() {
        if ( null == scInstance ) {
            scInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public void Update() {}

    public static Level GetCurrentLevel() {
        return ( currLevel );
    }

    public static void LoadLevel( Level level ) {
        currLevel = level;
    }
}