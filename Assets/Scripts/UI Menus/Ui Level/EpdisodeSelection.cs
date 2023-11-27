using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EpdisodeSelection : MonoBehaviour
{

   // [SerializeField] private TextMeshProUGUI m_CoinText;
    [SerializeField] private List<Button> LevelBtns;
    public LevelManager lvlManager;/*
    [SerializeField] private Button BackBtn;*/

    private void Start()
    {
        LockFrom(Dependencies.GameDataOperations.GetUnlockedEpisodes());
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
            print("dsfdf");
        }
    }

    public void OpenEpisode(int i)
    {
        print(Dependencies.GameDataOperations.GetSelectedEpisode());
        //LevelManager.Episode = i;
        Dependencies.GameDataOperations.SetSelectedEpisode(i);
        /*SaveLoadData.GameData.m_SelectedEpisode = i;*/
        
        lvlManager.OpenLevelPanel();
        
    }

}
