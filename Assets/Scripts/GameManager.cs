using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Start,
    Running,
    Frenzy,
    Pause,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action<GameState> ON_CHANGE_STATE;
    public GameState currentGameState;
    private bool isFrenzyMode;
    public bool IsFrenzyMode => isFrenzyMode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        ON_CHANGE_STATE += OnChangeState;
    }

    void OnDestroy()
    {
        ON_CHANGE_STATE -= OnChangeState;
    }

    public void OnChangeState(GameState newState)
    {
        if (newState == currentGameState)
            return;
        currentGameState = newState;
        isFrenzyMode = newState == GameState.Frenzy;
        switch (newState)
        {
            case GameState.Start:
                break;
            case GameState.Running:
                break;
             case GameState.Frenzy:
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }
}
