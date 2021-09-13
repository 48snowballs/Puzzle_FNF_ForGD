using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text textDisplay;

    private float timeRemaining = 10;
    private bool timerIsRunning;
    private bool overTime;

    private void Start()
    {
        timerIsRunning = false;
        overTime = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                StopCountDown();
            }
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        textDisplay.text = string.Format("{0:00}", timeToDisplay);
    }

    public void StartCountDown(float s)
    {
        textDisplay.gameObject.SetActive(true);
        overTime = false;
        timeRemaining = s;
        timerIsRunning = true;
    }
    public void StopCountDown()
    {
        textDisplay.gameObject.SetActive(false);
        if (timeRemaining > 0) overTime = false;
        else overTime = true;
        timeRemaining = 0;
        timerIsRunning = false;
    }
    public bool IsOverTime()
    {
        return overTime;
    }
}
