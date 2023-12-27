using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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
        GameEvents.GamePlayEvents.OnInterstitialClosed.Unregister(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Unregister(OnAdFailed);
    }

    private void OnAdFailed()
    {
        StartCoroutine(Wait());
    }

    protected override void OnMenuContainerEnable()
    {
        GameEvents.GamePlayEvents.OnInterstitialClosed.Register(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Register(OnAdFailed);
        IncrementProgressLevel();
        AdHandler.ShowInterstitial();
    }

    private void OnAdClosed()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        _animator.enabled = true;
        _animator.SetTrigger("Open");
        
    }

    protected override void OnMenuContainerDisable()
    {
        GameEvents.GamePlayEvents.OnLevelResumed.Raise();
        GameEvents.GamePlayEvents.OnInterstitialClosed.Unregister(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Unregister(OnAdFailed);
    }

    private void OnAllGroupsCleared()
    {
        StartCoroutine(GameWon());
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSecondsRealtime(2f);
        IncrementProgressLevel();
        ChangeMenuState(MenuName.GameplayLevelWon);
        
        GameEvents.GamePlayEvents.OnLevelPause.Raise();
        
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
