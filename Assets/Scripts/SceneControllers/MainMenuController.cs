using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuController : ISceneController
{
    override protected GameState GetGameState() { return GameState.MAIN_MENU; }

    [SerializeField] private Image mainImage;

    private void Start()
    {
        Sequence mainImageSequence = DOTween.Sequence();
        mainImageSequence.AppendInterval(0.5f)
            .Append(mainImage.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));

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
