using UnityEngine;
using TMPro;

public class GameOverController : ISceneController
{
    override protected GameState GetGameState() { return GameState.GAME_OVER_SCREEN; }

    [SerializeField] private TMP_Text scoreText;

    // Start is called before the first frame update
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
