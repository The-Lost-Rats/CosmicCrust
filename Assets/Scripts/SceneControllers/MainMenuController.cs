using UnityEngine;

public class MainMenuController : ISceneController
{
    override protected GameState GetGameState() { return GameState.MAIN_MENU; }

    private void Start()
    {
        if (!AudioController.Instance.IsMusicPlaying())
        {
            AudioController.Instance.PlayMusic(MusicKeys.MainMenu);
        }
    }

    override protected void SceneUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.instance.ChangeState(GameState.PLAY);
        }
    }
}
