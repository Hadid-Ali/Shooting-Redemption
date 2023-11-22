using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompleteLevel : UIMenuBase
{
    [SerializeField] private Text Cointxt;
    [SerializeField] private Button m_Nextbutton;
    [SerializeField] private Button m_MainMenubutton;
    private int levelsCompleted = 0;

    private void Start()
    {
        m_Nextbutton.onClick.AddListener(OnNextBtnPressed);
        m_MainMenubutton.onClick.AddListener(OnLoadScene);
    }

    private void OnEnable()
    {
        GameEvents.MenuEvents.LevelWin.Register(OnLevelComplete);
    }

    private void OnDisable()
    {
        GameEvents.MenuEvents.LevelWin.Unregister(OnLevelComplete);
    }

    public void OnLevelComplete()
    {
        ChangeMenuState(MenuName.LevelComplete);
        SaveLoadData.SaveData();
        levelsCompleted++;
        if (levelsCompleted % 5 == 0)
        {
            SceneManager.LoadScene("NextLevelScene");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    public void OnNextBtnPressed()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void updateCoinsTxt()
    {
        Cointxt.text = $"Coins: {SaveLoadData.GameData.m_Coins}";
    }
    private void OnLoadScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
