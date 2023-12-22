using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWinPanel : UIMenuBase
{
    public static GameEvent<GameWinStats> OnUpdateStatsStart = new();
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Register(OnAllGroupsCleared);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnAllGroupsCleared.Unregister(OnAllGroupsCleared);
    }

    protected override void OnMenuContainerEnable()
    {
        AdHandler.ShowInterstitial();
        Time.timeScale = 0.001f;
        
        IncrementProgressLevel();
        GameEvents.GamePlayEvents.OnInterstitialClosed.Register(OnAdClosed);
    }

    private void OnAdClosed()
    {
        _animator.enabled = true;
        _animator.SetTrigger("Open");
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1;
        
    }

    private void OnAllGroupsCleared()
    {
        StartCoroutine(GameWon());
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(2f);
        ChangeMenuState(MenuName.GameplayLevelWon);
        IncrementProgressLevel();
        
        GameWinStats stats = new();
        stats.coinsEarned = (AiGroup.GetAllEnemiesCount() * 50) + (SessionData.Instance.civiliansKilled * 20);
        stats.previousCoins = Dependencies.GameDataOperations.GetCredits();
        stats.CiviliansKilled = SessionData.Instance.civiliansKilled;
        stats.EnemiesKilled = AiGroup.GetAllEnemiesCount();

        OnUpdateStatsStart.Raise(stats);
        
        Dependencies.GameDataOperations.AddCredits(stats.coinsEarned);
        Dependencies.GameDataOperations.SaveData();
    }
    
    public void IncrementProgressLevel()
    {
        int currentLevel = Dependencies.GameDataOperations.GetSelectedLevel();
        int currentEpisode = Dependencies.GameDataOperations.GetSelectedEpisode();
        
        int unlockedLevels = currentLevel;
        
        if (unlockedLevels < 4)
        {
            unlockedLevels = currentLevel + 1;
            if(unlockedLevels > Dependencies.GameDataOperations.GetUnlockedLevels(currentEpisode))
                Dependencies.GameDataOperations.SetUnlockedLevels(currentEpisode, unlockedLevels);
        }
        if (unlockedLevels >= 4 && currentEpisode < 4)
        {
            int nextEpisode = currentEpisode + 1;
            if (!Dependencies.GameDataOperations.GetUnlockedEpisodes(nextEpisode))
            {
                Dependencies.GameDataOperations.SetUnlockedEpisodes(nextEpisode);
            }
            
        }
        Dependencies.GameDataOperations.SetSelectedLevel(currentLevel);
        Dependencies.GameDataOperations.SaveData();
    }
    
}
