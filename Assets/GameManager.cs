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
}

public class GameManager : MonoBehaviour
{
    //REFERENCES 
    public ShootinSpotsManager spotMng;

    public GamePhases gamePhase = GamePhases.Gameplay;
    public GamePlayState gameplayState = GamePlayState.ReadyToShoot;

    public float shotDuration = 3f;
    
    void OnEnable()
    {
        GameEvents.OnShootStarted += HandleShoot;
        GameEvents.OnShotFinished += HandleShotFinished;
    }

    void Start()
    {
        if(!spotMng) spotMng = GetComponent<ShootinSpotsManager>();
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
    }

    void StartStandby()
    {
        gamePhase = GamePhases.Standby;
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
