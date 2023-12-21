using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : UIMenuBase
{
    [SerializeField] private WeaponSelection _weaponSelection;
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
                _weaponSelection.ScrollGun(false);
                break;
            case ButtonType.ScrollRight:
                _weaponSelection.ScrollGun(true);
                break;
            case ButtonType.Buy:
                _weaponSelection.BuyGun();
                break;
            case ButtonType.Select:
                _weaponSelection.SelectGun();
                break;
            case ButtonType.TryForFree:
                AdHandler.ShowRewarded(_weaponSelection.OnRewardedGunADWatched);;
                break;
        }
    }
    

    


    
}
