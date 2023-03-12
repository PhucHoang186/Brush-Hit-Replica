using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    private static readonly int Transition_In = Animator.StringToHash("Transition_In");
    private static readonly int Transition_Out = Animator.StringToHash("Transition_Out");

    public static UIManager Instance;
    public static Action TRANSITION_IN;
    public static Action TRANSITION_OUT;
    public ScoreUIPanel scoreUIPanel;
    public LevelShowcase levelShowcase;

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
    }

    void Destroy()
    {
        TRANSITION_IN -= TransitionIn;
        TRANSITION_OUT -= TransitionOut;
    }

    public void ResetUI()
    {
        tapToStartText.SetActive(true);
    }

    public void TurnOffTapToStartText()
    {
        tapToStartText.SetActive(false);
    }

    public void OnReplayButtonClicked()
    {
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Lose);
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
