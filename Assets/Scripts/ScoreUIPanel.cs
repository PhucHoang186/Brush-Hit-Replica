using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIPanel : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = score.ToString();
            if (score == GameManager.Instance.GetLevelMaxScore())
            {
                GameManager.ON_CHANGE_STATE?.Invoke(GameState.TransitionToNextLevel);
            }
        }
    }
}
