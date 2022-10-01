using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Code From: https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas
public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject HelpMenu;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuButton();
    }

    public void ResumeButton()
    {
        SceneManager.UnloadSceneAsync("PauseMenuScene");
    }

    public void HelpButton()
    {
        // Show Help Menu
        PauseMenu.SetActive(false);
        HelpMenu.SetActive(true);
    }

    public void PauseMenuButton()
    {
        // Show Main Menu
        PauseMenu.SetActive(true);
        HelpMenu.SetActive(false);
    }
}