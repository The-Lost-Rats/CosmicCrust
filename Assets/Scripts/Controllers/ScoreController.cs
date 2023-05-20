using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController scInstance = null;

    private int currentScore;

    private int highScore;

    private string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        if (scInstance == null)
        {
            scInstance = this;
        }
        else if (scInstance != this)
        {
            Destroy(gameObject);
        }

        currentScore = 0;
        highScore = LoadScore();
    }

    private int LoadScore()
    {
        return (PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0));
    }

    public int GetHighScore()
    {
        return (highScore);
    }

    public int GetCurrentScore()
    {
        return (currentScore);
    }

    public void SetScore(int score)
    {
        currentScore = score;
    }

    public int UpdateScore(int amount)
    {
        currentScore += amount;
        return (currentScore);
    }

    public bool SaveHighScore()
    {
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, currentScore);
            return (true);
        }

        return (false);
    }

    public void WipeHighScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
    }
}
