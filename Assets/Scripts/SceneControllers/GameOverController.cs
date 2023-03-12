using UnityEngine;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
        scoreText.text = "" + PlayController.instance.score;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1.0f;
            GameController.instance.ChangeState(GameState.MAIN_MENU);
        }
    }
}
