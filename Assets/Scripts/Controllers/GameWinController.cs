using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinController : MonoBehaviour
{
    public static GameWinController gwcInstance = null;

    [SerializeField]
    private GameObject gameWinOverlay;

    public void Awake() {
        if ( null == gwcInstance ) {
            gwcInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }

        gameWinOverlay.SetActive(false);
    }

    void Update()
    {
        if (SceneController.GetCurrentLevel() == SceneController.Level.WIN_MENU)
        {
            if (!gameWinOverlay.activeSelf)
            {
                gameWinOverlay.SetActive(true);
            }

            if ( Input.GetKeyDown( KeyCode.Return ) ) {
                gameWinOverlay.SetActive(false);
            }
        }
    }
}