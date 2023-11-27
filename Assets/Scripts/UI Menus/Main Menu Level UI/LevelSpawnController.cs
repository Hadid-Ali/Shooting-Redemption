using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class LevelSpawnController : MonoBehaviour
{
    /*public LevelDataController[] levelImages;
    private void Start()
    {
        UpdateLevelImages();
    }

    private void UpdateLevelImages()
    {
        int levelsPerSeason = 6;
        int totalLevelsPlayed = SaveLoadData.GameData.m_Levels;

        int currentSeason = (totalLevelsPlayed - 1) / levelsPerSeason;
        int levelsInCurrentSeason = (totalLevelsPlayed - 1) % levelsPerSeason;

        for (int i = 0; i < levelImages.Length; i++)
        {
            LevelDataController levelImage = levelImages[i];

            int levelIndex = currentSeason * levelsPerSeason + i + 1;

            if (levelIndex <= totalLevelsPlayed)
            {
                if (levelIndex == totalLevelsPlayed)
                {
                    levelImage.SetStatus(LevlsTypeSTatus.Doing, i + 1);
                }
                else if (levelIndex < totalLevelsPlayed)
                {
                    levelImage.SetStatus(LevlsTypeSTatus.Done, i + 1);
                }
                else
                {
                    levelImage.SetStatus(LevlsTypeSTatus.todo, i + 1);
                }
            }
            else if (levelIndex % levelsPerSeason == 0)
            {
                levelImage.SetStatus(LevlsTypeSTatus.Boss, i + 1);
            }
            else
            {
                levelImage.SetStatus(LevlsTypeSTatus.todo, i + 1);
            }
        }

        int levelsRemaining = levelsPerSeason - levelsInCurrentSeason;
        Debug.Log($"Levels in current season: {levelsInCurrentSeason + 1}"); // Add 1 to index
        Debug.Log($"Total levels played: {totalLevelsPlayed}");
        Debug.Log($"Levels remaining in current season: {levelsRemaining}");
    }*/
    
    
    
    


    /*public int GenerateLevelNumber(int n,int m)
    {
        return n/m;
    }*/
}
