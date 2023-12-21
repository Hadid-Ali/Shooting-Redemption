using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFailPanel : UIMenuBase
{
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
    }

    protected override void OnMenuContainerDisable()
    {
        Time.timeScale = 1f;
    }



}
