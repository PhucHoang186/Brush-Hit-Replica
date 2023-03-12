using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum GameState
{
    Start,
    LoadLevel,
    WaitForInput,
    Running,
    Frenzy,
    Lose,
    TransitionToNextLevel,
    Pause,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action<GameState> ON_CHANGE_STATE;
    public GameState currentGameState;
    public LevelsConfig levelsConfig;
    public PlatformContainer platformContainer;
    private bool isFrenzyMode;
    private int failCount;
    public bool IsFrenzyMode => isFrenzyMode;
    private int currentLevel = 1;
    private int levelSection = 0;
    private int levelMaxScore;
    private bool isInitPlay = true;
    private string currentLevelName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        ON_CHANGE_STATE += OnChangeState;
        DOVirtual.DelayedCall(0.5f, () => ON_CHANGE_STATE?.Invoke(GameState.Start));
    }

    void OnDestroy()
    {
        ON_CHANGE_STATE -= OnChangeState;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentGameState == GameState.WaitForInput)
            {
                ON_CHANGE_STATE?.Invoke(GameState.Running);
            }
        }
    }
    public void OnChangeState(GameState newState)
    {
        // if (newState == currentGameState)
        //     return;
        currentGameState = newState;
        isFrenzyMode = newState == GameState.Frenzy;
        switch (newState)
        {
            case GameState.Start:
                ON_CHANGE_STATE?.Invoke(GameState.LoadLevel);
                break;
            case GameState.LoadLevel:
                LoadLevel();
                break;
            case GameState.Running:
                break;
            case GameState.Frenzy:
                break;
            case GameState.Lose:
                EndGame();
                break;
            case GameState.TransitionToNextLevel:
                ToNextlevel();
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }

    private void ToNextlevel()
    {
        StartCoroutine(CorToNextLevel());
    }

    private IEnumerator CorToNextLevel()
    {
        yield return new WaitForSeconds(2f);
        UIManager.TRANSITION_IN?.Invoke();
        yield return new WaitForSeconds(3f);
        ON_CHANGE_STATE?.Invoke(GameState.Start);
    }

    private void EndGame()
    {
        StartCoroutine(CorEndGame());
    }

    private IEnumerator CorEndGame()
    {
        currentLevel = 1;
        levelSection = 0;
        UIManager.Instance.scoreUIPanel.Score = 0;
        yield return new WaitForSeconds(2f);
        UIManager.TRANSITION_IN?.Invoke();
        yield return new WaitForSeconds(3f);
        UIManager.Instance.ResetUI();
        ON_CHANGE_STATE?.Invoke(GameState.Start);
    }

    private void LoadLevel()
    {
        StartCoroutine(CorLoadLevel());

    }

    public int GetLevelMaxScore()
    {
        return levelMaxScore;
    }

    private IEnumerator CorLoadLevel()
    {

        if (!String.IsNullOrEmpty(currentLevelName))
            SceneManager.UnloadSceneAsync(currentLevelName);

        var levelData = levelsConfig.GetLevelData(currentLevel);
        if (levelSection <= levelData.GetTotalNumber())
        {
            levelSection++;
        }
        else
        {
            currentLevel++;
            levelSection = 1;
            levelData = levelsConfig.GetLevelData(currentLevel);
        }

        UIManager.Instance.collectItemUI.ResetCollectItems();
        levelMaxScore = levelData.levelMaxScore;
        currentLevelName = levelData.GetLevelName(levelSection);
        var async = SceneManager.LoadSceneAsync(levelData.GetLevelName(levelSection), LoadSceneMode.Additive);
        while (!async.isDone)
            yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevelName));
        UIManager.TRANSITION_OUT?.Invoke();
        ON_CHANGE_STATE?.Invoke(GameState.WaitForInput);
    }
}
