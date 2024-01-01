using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFailPanel : UIMenuBase
{
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        GameEvents.GamePlayEvents.OnPlayerDead.Register(GameLost);
    }

    private void OnDestroy()
    {
        GameEvents.GamePlayEvents.OnPlayerDead.Unregister(GameLost);
        GameEvents.GamePlayEvents.OnInterstitialClosed.Unregister(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Unregister(OnAdFailed);
    }

    private void GameLost()
    {
        ChangeMenuState(MenuName.GameplayLevelFailed);
        
        string episode = "Episode : " + Dependencies.GameDataOperations.GetSelectedEpisode();
        string level = "Level : " + Dependencies.GameDataOperations.GetSelectedLevel();
        FirebaseEvents.logEvent($"{episode} {level} Lost");
    }
    protected override void OnMenuContainerEnable()
    {
        AdHandler.ShowInterstitial();
        GameEvents.GamePlayEvents.OnInterstitialClosed.Register(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Register(OnAdFailed);
        Time.timeScale = 0.001f;
    }

    private void OnAdClosed()
    {
        _animator.enabled = true;
        _animator.SetTrigger("Open");
        GameEvents.GamePlayEvents.OnLevelPause.Raise();
    }
    protected override void OnMenuContainerDisable()
    {
        _animator.enabled = false;
        GameEvents.GamePlayEvents.OnLevelResumed.Raise();
        GameEvents.GamePlayEvents.OnInterstitialClosed.Unregister(OnAdClosed);
        GameEvents.GamePlayEvents.OnInterstitialFailed.Unregister(OnAdFailed);
        Time.timeScale = 1f;
    }

    private void OnAdFailed()
    {
        _animator.enabled = true;
        _animator.SetTrigger("Open");
        GameEvents.GamePlayEvents.OnLevelPause.Raise();
    }
}
