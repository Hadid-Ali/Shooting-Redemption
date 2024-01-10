using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : UIMenuBase
{
    private void Awake()
    {
        Time.timeScale = 1;
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
    }

    private void Start()
    {
        Dependencies.SoundHandler.PlayBGMusic();

        if (SessionData.Instance.comingToMainMenuOnModeComplete)
        {
            SessionData.Instance.comingToMainMenuOnModeComplete = false;
            ChangeMenuState(MenuName.EpisodesSelection);
        }
    }

    protected override void OnMenuContainerEnable()
    {
        Time.timeScale = 1;
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
    }


    protected override void OnMenuContainerDisable()
    {
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }
    

    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.Play:
                ChangeMenuState(MenuName.EpisodesSelection);
                break;
            case ButtonType.CharactersPanel:
                ChangeMenuState(MenuName.CharacterSelection);
                break;
            case ButtonType.GunsPanel:
                ChangeMenuState(MenuName.GunSelection);
                break;
            case ButtonType.AddCoins:
                AdHandler.ShowRewarded(OnRewardedAddCoins);
                break;
            case ButtonType.Settings:
                ChangeMenuState(MenuName.SettingsMenu);
                break;
            case ButtonType.PrivacyPolicy:
                Application.OpenURL("https://play.virtua.com/privacy-policy");
                break;
        }
        print("Working");
    }
    private void OnRewardedAddCoins()
    {
        Dependencies.GameDataOperations.AddCredits(200);
    }

    
}
