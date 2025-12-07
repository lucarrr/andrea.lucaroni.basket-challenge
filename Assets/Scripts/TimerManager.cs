using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;       // Assign in Inspector
    public float totalTime = 60; // seconds

    private float timeRemaining;
    private bool isRunning = false;
    private int lastDisplayedSeconds = -1;

    public event Action OnTimerEnd; // Game over event

    public void StartTimer()
    {
        timeRemaining = totalTime;
        isRunning = true;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            UpdateTimerUI();
            OnTimerEnd?.Invoke(); // Call Game Over
            return;
        }


        // Only update when the displayed second changes
        int currentSeconds = Mathf.FloorToInt(timeRemaining);
        if (currentSeconds != lastDisplayedSeconds)
        {
            lastDisplayedSeconds = currentSeconds;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        // Format as MM:SS
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}