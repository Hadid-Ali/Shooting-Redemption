using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPausePanel : UIMenuBase
{
    [SerializeField] private Toggle sound;
    private static readonly int Play = Animator.StringToHash("Play");

    [SerializeField] private Animator _animator;

    protected override void OnMenuContainerEnable()
    {
        sound.isOn = Dependencies.GameDataOperations.GetSoundStatus();
        
        AdHandler.ShowInterstitial();
        CharacterStates.gameState = GameStates.GamePause;
        GameEvents.GamePlayEvents.OnInterstitialClosed.Register(OnAdClosed);

    }

    private void OnAdClosed()
    {
        _animator.enabled = true;
        _animator.SetTrigger(Play);
        GameEvents.GamePlayEvents.OnLevelPause.Raise();
        Time.timeScale = 0.001f;
    }

    protected override void OnMenuContainerDisable()
    {
        _animator.enabled = false;
        CharacterStates.gameState = GameStates.InGame;
        GameEvents.GamePlayEvents.OnInterstitialClosed.Unregister(OnAdClosed);
        GameEvents.GamePlayEvents.OnLevelResumed.Raise();
        Time.timeScale = 1f;
    }
    
}
