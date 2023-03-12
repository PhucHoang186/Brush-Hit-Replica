using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] GameObject newLevel;
    [SerializeField] GameObject currentLevel;
    [SerializeField] GameObject clearedLevel;

    public void SetNewLevel()
    {
        newLevel.SetActive(true);
        currentLevel.SetActive(false);
        clearedLevel.SetActive(false);
    }

    public void SetCurrentLevel()
    {
        newLevel.SetActive(false);
        currentLevel.SetActive(true);
        clearedLevel.SetActive(false);
    }

    public void SetClearedLevel()
    {
        newLevel.SetActive(false);
        currentLevel.SetActive(false);
        clearedLevel.SetActive(true);
    }
}
