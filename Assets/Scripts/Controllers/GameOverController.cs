using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Code From: https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas
public class GameOverController : MonoBehaviour
{
    public GameObject GameOverMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen();
    }

    public void StartButton()
    {
        // Start button has been pressed
        SceneManager.UnloadSceneAsync("GameOverScene");
    }

    public void GameOverScreen()
    {
        // Show Main Menu
        GameOverMenu.SetActive(true);
    }
}