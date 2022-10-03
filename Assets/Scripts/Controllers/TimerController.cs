using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code from: https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
public class TimerController : MonoBehaviour
{
    public static TimerController tcInstance = null;

    [SerializeField]
    public float startTime = 10;

    private float timeRemaining = 0;

    public bool timerIsRunning = false;
    
    [SerializeField]
    public GameObject timerSprite;

    [SerializeField]
    private List<Sprite> numerics;

    public void Awake() {
        if ( null == tcInstance ) {
            tcInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public void StartTimer()
    {
        timeRemaining = startTime;
        timerIsRunning = true;
    }

    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    public void ResumeTimer()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime * BeltController.bcInstance.GetSpeed();
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        
        timerSprite.GetComponent<SpriteRenderer>().sprite = numerics[seconds];
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
    }

    public bool IsDone()
    {
        return (timeRemaining == 0);
    }
}