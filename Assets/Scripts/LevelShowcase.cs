using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelShowcase : MonoBehaviour
{
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] Transform levelUIContainer;
    [SerializeField] LevelUI levelUIPrefab;
    // private List<LevelUI> levelUIs = new List<LevelUI>();
    private GameManager gameManager;

    public void InitLevelShowcasePanel()
    {
        for (int i = 0; i < levelUIContainer.childCount; i++)
        {
            Destroy(levelUIContainer.GetChild(i).gameObject);
        }

        if (GameManager.Instance)
        {
            // levelUIs.Clear();
            gameManager = GameManager.Instance;
            currentLevelText.text = gameManager.GetCurrentLevel().ToString();
            nextLevelText.text = (gameManager.GetCurrentLevel() + 1).ToString();
            var totalLevelSection = gameManager.GetCurrentTotalLevelSection();
            for (int i = 0; i < totalLevelSection; i++)
            {
                var newLevelUI = Instantiate(levelUIPrefab, levelUIContainer);
                // levelUIs.Add(newLevelUI);
                if (i == gameManager.GetCurrentLevelSection() - 1)
                {
                    newLevelUI.SetCurrentLevel();
                }
                else if (i < gameManager.GetCurrentLevelSection() - 1)
                {
                    newLevelUI.SetClearedLevel();
                }
            }

        }
    }
}
