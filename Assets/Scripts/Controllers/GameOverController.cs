using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public static GameOverController gocInstance = null;

    [SerializeField]
    private GameObject gameOverOverlay;

    [SerializeField]
    private GameObject scoreTens;

    [SerializeField]
    private GameObject scoreOnes;

    public void Awake() {
        if ( null == gocInstance ) {
            gocInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }

        gameOverOverlay.SetActive(false);
    }

    void Update()
    {
        if (SceneController.GetCurrentLevel() == SceneController.Level.GAME_OVER_MENU)
        {
            if (!gameOverOverlay.activeSelf)
            {
                gameOverOverlay.SetActive(true);
            }

            if ( Input.GetKeyDown( KeyCode.Return ) ) {
                gameOverOverlay.SetActive(false);
            }
        }
    }

    public void UpdateFinalScore(int score)
    {
        int tens = Mathf.FloorToInt(score / 10);
        int ones = Mathf.FloorToInt(score % 10);

        scoreTens.GetComponent<SpriteRenderer>().sprite = UIController.uicInstance.digits[tens];
        scoreOnes.GetComponent<SpriteRenderer>().sprite = UIController.uicInstance.digits[ones];
    }

}