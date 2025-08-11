using System.Collections;
using System;
using UnityEngine;

public enum GamePhases
{
    Standby,
    Gameplay,
    EndScreen,
}

public enum GamePlayState
{
    ReadyToShoot,
    Shooting,
    BallInTheAir,
    Relocating,
    Cooldown,
}

public class GameEvents
{
    public static Action OnShootStarted;
    public static Action OnShotFinished;
    public static Action OnRelocatePlayer;
    public static Action OnGameStarted;
    public static Action OnGameEnded;
}

public class GameManager : MonoBehaviour
{
    //REFERENCES 
    [SerializeField] private ShootinSpotsManager spotMng;
    [SerializeField] private TimerManager timerManager;

    public static GamePhases gamePhase = GamePhases.Standby;
    public static GamePlayState gameplayState;

    public float shotDuration = 2.5f;
    
    void OnEnable()
    {
        GameEvents.OnShootStarted += HandleShoot;
        GameEvents.OnShotFinished += HandleShotFinished;
        GameEvents.OnGameStarted += StartGame;
    }

    void Start()
    {
        if(!spotMng) spotMng = GetComponent<ShootinSpotsManager>();
        if(!timerManager) timerManager = GetComponent<TimerManager>();
    }

    void Update()
    {
        switch (gamePhase)
        {
            case GamePhases.Standby:
                // For now it starts with space -- ToDo UI Buttons
                if (Input.GetKeyDown(KeyCode.Space))
                    StartGame();
                break;

            case GamePhases.Gameplay:
                UpdateGameplay();
                break;

            case GamePhases.EndScreen:
                if (Input.GetKeyDown(KeyCode.Space))
                    StartStandby();
                break;

        }       
    }


    void UpdateGameplay()
    {
        switch (gameplayState)
        {
            case GamePlayState.ReadyToShoot:
                //Allow Shooter.cs to shoot
                break;
            
             case GamePlayState.Shooting:
                //Switch the camera so it follows
                gameplayState = GamePlayState.BallInTheAir;
                break;
             case GamePlayState.BallInTheAir:
                //Camera is following
                shotDuration -= Time.deltaTime;
                if (shotDuration <= 0f)
                {
                    GameEvents.OnShotFinished.Invoke();
                }
                break;
            case GamePlayState.Relocating:
                spotMng.SwitchPos();
                //Camera Relocation
                gameplayState = GamePlayState.Cooldown;
                break;
            case GamePlayState.Cooldown:
                //Just wait
                gameplayState= GamePlayState.ReadyToShoot;
                break;

        }
    }

    void StartGame()
    {
        gamePhase = GamePhases.Gameplay;
        gameplayState = GamePlayState.ReadyToShoot;

        timerManager.StartTimer();
        timerManager.OnTimerEnd += GameEnd;
    }

    void StartStandby()
    {
        gamePhase = GamePhases.Standby;
    }

    void GameEnd()
    {
        gamePhase = GamePhases.EndScreen;
        GameEvents.OnGameEnded?.Invoke();  
    }

    void HandleShoot()
    {
        gameplayState =  GamePlayState.Shooting;
    }

    void HandleShotFinished()
    {
        Debug.Log("Shot finished");
        ShotTimerReset();
        gameplayState = GamePlayState.Relocating;
    }

    void ShotTimerReset()
    {
        shotDuration = 3f;
    }
}
