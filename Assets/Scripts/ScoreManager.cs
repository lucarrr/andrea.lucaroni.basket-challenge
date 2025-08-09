using System.Collections;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore;
    public event Action<int> OnScoreChanged;

    private int pointForClean = 3, pointForBackboard = 2, pointForDirty = 2;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void AddPoints(int points)
    {
        CurrentScore += points;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void BallInBasket(BallBehaviour.BallState ball)
    {   
        int p = 0;

        switch (ball)
        {   
            case BallBehaviour.BallState.TouchedBackboard:
                p = pointForBackboard;
                break;
            case BallBehaviour.BallState.CleanShot:
                p = pointForClean;
                break;
            case BallBehaviour.BallState.TouchedRim:
                p = pointForDirty;
                break;
        }

        AddPoints(p);
       
    }


}
