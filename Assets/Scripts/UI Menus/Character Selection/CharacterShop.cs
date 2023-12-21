using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterShop : UIMenuBase
{
    [SerializeField] private CharacterHandler _characterSelection;



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
            case ButtonType.Exit:
                ChangeMenuState(MenuName.MainMenu);
                break;
            case ButtonType.AddCoins:
                AdHandler.ShowRewarded(() => Dependencies.GameDataOperations.AddCredits(200));
                break;
            case ButtonType.ScrollLeft:
                _characterSelection.ScrollGun(false);
                break;
            case ButtonType.ScrollRight:
                _characterSelection.ScrollGun(true);
                break;
            case ButtonType.Buy:
                _characterSelection.BuyGun();
                break;
            case ButtonType.Select:
                _characterSelection.SelectGun();
                break;
            case ButtonType.TryForFree:
                AdHandler.ShowRewarded(_characterSelection.OnRewardedGunADWatched);;
                break;
        }
    }



}
