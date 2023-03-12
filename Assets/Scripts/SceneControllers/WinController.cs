using UnityEngine;
using TMPro;

public class WinController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        Time.timeScale = 0.0f;
        scoreText.text = "" + PlayController.instance.score;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1.0f;
            GameController.instance.ChangeState(GameState.MAIN_MENU);
        }
    }
}
