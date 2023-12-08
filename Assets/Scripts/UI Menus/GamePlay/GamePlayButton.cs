using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GamePlayButtonName
{
    MainMenu,
    Restart,
    Next,
    Pause,
    Exit,
    DoubleReward
}

[RequireComponent(typeof(Button))]
public class GamePlayButton : MonoBehaviour
{
    [SerializeField] private GamePlayButtonName buttonName;
    private Button _button;

    public UnityEvent buttonEvent;
    
 

    private void Start()
    {
        _button = GetComponent<Button>();
        
        switch (buttonName)
        {
            case GamePlayButtonName.MainMenu:
                _button.onClick.AddListener(OnMenuMenuClicked);
                break;
            case GamePlayButtonName.Restart:
                _button.onClick.AddListener(OnRestartClicked);
                break;
            case GamePlayButtonName.Next:
                _button.onClick.AddListener(OnNextLevelClicked);

                if (Dependencies.GameDataOperations.GetSelectedLevel() >= 4)
                    _button.interactable = false;
                
                break;
            case GamePlayButtonName.Pause:
                _button.onClick.AddListener(OnPauseButtonClicked);
                break;
            case GamePlayButtonName.Exit:
                _button.onClick.AddListener(OnExitButtonClicked);
                break;
            case GamePlayButtonName.DoubleReward:
                _button.onClick.AddListener(OnClickDoubleReward);
                break;
                
        }
    }

    private void OnClickDoubleReward()
    {
        AdHandler.ShowRewarded(OnClickDoubleRewardCallBack);
    }

    private void OnClickDoubleRewardCallBack()
    {
        _button.interactable = false;
        
        GameWinStats stats = new();
        stats.coinsEarned = ((AiGroup.GetAllEnemiesCount() * 50)  ) + Dependencies.GameDataOperations.GetCredits();
        stats.previousCoins = Dependencies.GameDataOperations.GetCredits();
        stats.CiviliansKilled = SessionData.Instance.civiliansKilled;
        stats.EnemiesKilled = AiGroup.GetAllEnemiesCount();

        LevelWinPanel.OnUpdateStatsStart.Raise(stats);
        
        Dependencies.GameDataOperations.SetCredit(stats.coinsEarned);
        Dependencies.GameDataOperations.SaveData();
    }

    private void OnMenuMenuClicked()
    {
        buttonEvent?.Invoke();
        SessionData.Instance.sceneToLoad = SceneName.MainMenu;
        SceneManager.LoadScene("LoadingScreen");
    }

    private void OnRestartClicked()
    {
        buttonEvent?.Invoke();

        SceneManager.LoadScene("LoadingScreen");
    }

    private void OnNextLevelClicked()
    {
        int currentLevel = Dependencies.GameDataOperations.GetSelectedLevel();
        currentLevel++;
        Dependencies.GameDataOperations.SetSelectedLevel(currentLevel);
        
        SceneManager.LoadScene("LoadingScreen");
        
    }
    private void OnPauseButtonClicked()
    {
        buttonEvent?.Invoke();
        GameplayPausePanel.ChangeMenuState(MenuName.GameplayPause);
        
        Time.timeScale = 0.001f;
        
    }

    private void OnExitButtonClicked()
    {
        buttonEvent?.Invoke();
        GameplayPausePanel.ChangeMenuState(MenuName.None);
        Time.timeScale = 1f;
        
    }
}
