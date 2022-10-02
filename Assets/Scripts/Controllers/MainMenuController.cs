using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code From: https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas
public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject HelpMenu;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void StartButton()
    {
        // Start button has been pressed
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void HelpButton()
    {
        // Show Help Menu
        MainMenu.SetActive(false);
        HelpMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        HelpMenu.SetActive(false);
    }
}