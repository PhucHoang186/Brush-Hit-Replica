using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static readonly int Transition_In = Animator.StringToHash("Transition_In");
    private static readonly int Transition_Out = Animator.StringToHash("Transition_Out");

    public static UIManager Instance;
    public static Action TRANSITION_IN;
    public static Action TRANSITION_OUT;
    public ScoreUIPanel scoreUIPanel;
    public LevelShowcase levelShowcase;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    public CollectItemUIContainer collectItemUI;
    [SerializeField] GameObject tapToStartText;
    [SerializeField] Animator transitionAnimator;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        TRANSITION_IN += TransitionIn;
        TRANSITION_OUT += TransitionOut;
        GameManager.ON_CHANGE_STATE += OnChangeState;
    }

    void OnDestroy()
    {

        TRANSITION_IN -= TransitionIn;
        TRANSITION_OUT -= TransitionOut;
        GameManager.ON_CHANGE_STATE -= OnChangeState;
    }

    public void ResetUI()
    {
        tapToStartText.SetActive(true);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
    }

    public void TurnOffTapToStartText()
    {
        tapToStartText.SetActive(false);
    }

    public void OnChangeState(GameState newState)
    {
        switch (newState)
        {

            case GameState.Lose:
                ShowGameOverScreen();
                break;
            case GameState.TransitionToNextLevel:
                break;
            case GameState.Pause:
                break;
            default:
                break;
        }
    }

    public void OnReplayButtonClicked()
    {
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Replay);
    }

    private void ShowGameOverScreen()
    {
        DOVirtual.DelayedCall(2f, () => gameOverScreen.gameObject.SetActive(true));
    }

    public void TransitionIn()
    {
        transitionAnimator.CrossFade(Transition_In, 0.25f);
    }

    public void TransitionOut()
    {
        transitionAnimator.CrossFade(Transition_Out, 0.25f);
    }
}
