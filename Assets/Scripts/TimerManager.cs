using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;       // Assign in Inspector
    public float totalTime = 60; // seconds

    private float timeRemaining;
    private bool isRunning = false;

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
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        // Format as MM:SS
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}