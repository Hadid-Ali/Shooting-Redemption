using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private List<Button> LevelBtns;
    public LevelManager lvlManager;

    /*private void Update()
    {
        LockFrom(SaveLoadData.GameData.m_UnLockLevel);
    }*/
    private void OnEnable()
    {
        print(Dependencies.GameDataOperations.GetSelectedEpisode());
        print(Dependencies.GameDataOperations.GetUnlockedLevels(Dependencies.GameDataOperations.GetSelectedEpisode()));
        LockFrom(Dependencies.GameDataOperations.GetUnlockedLevels(Dependencies.GameDataOperations.GetSelectedEpisode()));
        
    }

    void LockFrom(int StartFrom)
    {
        for (int i = 0; i < LevelBtns.Count; i++)
        {
            LevelBtns[i].interactable = true;
        }

        for (int i = StartFrom; i < LevelBtns.Count; i++)
        {
            LevelBtns[i].interactable = false;
        }
    }


    public void OpenLevel(int i)
    {
        print(i+"asfsf");
        Dependencies.GameDataOperations.SetSelectedlevel(i);
       // LevelManager.Level = i;
        lvlManager.SetLevelNum();
        /*SaveLoadData.GameData.m_SelectedLevel = i;*/
        
        
        
        
        
    }
}
