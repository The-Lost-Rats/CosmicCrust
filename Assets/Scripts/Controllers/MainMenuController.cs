using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) ) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}