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
    }

    private void GameLost()
    {
        ChangeMenuState(MenuName.GameplayLevelFailed);
    }
    protected override void OnMenuContainerEnable()
    {
        AdHandler.ShowInterstitial();
        Time.timeScale = 0.001f;
        GameEvents.GamePlayEvents.OnInterstitialClosed.Register(OnAdClosed);
    }

    private void OnAdClosed()
    {
        _animator.enabled = true;
        _animator.SetTrigger("Play");
    }
    protected override void OnMenuContainerDisable()
    {
        _animator.enabled = false;
        Time.timeScale = 1f;
    }



}
