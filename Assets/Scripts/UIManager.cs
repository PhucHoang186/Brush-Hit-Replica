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

    public CollectItemUIContainer collectItemUI;
    [SerializeField] Button tapToStartButton;
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
        tapToStartButton.transform.gameObject.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        tapToStartButton.transform.gameObject.SetActive(false);
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Running);
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
