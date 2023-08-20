using UnityEngine;
using TMPro;

public class WinController : ISceneController
{
    override protected GameState GetGameState() { return GameState.WIN_SCREEN; }

    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        Time.timeScale = 0.0f;
        scoreText.text = "" + ScoreController.scInstance.GetCurrentScore();
    }

    override protected void SceneUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1.0f;
            GameController.instance.ChangeState(GameState.MAIN_MENU);
        }
    }
}
