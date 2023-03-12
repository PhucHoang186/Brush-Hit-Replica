using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Configs/Level")]
public class LevelsConfig : ScriptableObject
{
    public List<LevelData> levelDatas;

    public LevelData GetLevelData(int level)
    {
        foreach (var LevelData in levelDatas)
            if (LevelData.level == level)
            {
                return LevelData;
            }
        Debug.LogError("Can't not find this level " + level);
        return null;
    }
}

[Serializable]
public class LevelData
{
    public int level;
    public int totalLevelSectionNumber;
    public List<int> levelMaxScoreForeachSection; // temporary, will add another way to track when have time

    public string GetLevelName(int levelNumber)
    {
        return "Level_" + level + "_" + levelNumber;
    }

    public int GetTotalNumber()
    {
        return totalLevelSectionNumber;
    }

}


