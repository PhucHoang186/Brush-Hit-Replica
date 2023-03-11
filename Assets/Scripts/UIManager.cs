using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public ScoreUIPanel scoreUIPanel;
    public CollectItemUIContainer collectItemUI;
    [SerializeField] Button tapToStartButton;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnStartButtonClicked()
    {
        tapToStartButton.transform.gameObject.SetActive(false);
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Running);
    }
}
