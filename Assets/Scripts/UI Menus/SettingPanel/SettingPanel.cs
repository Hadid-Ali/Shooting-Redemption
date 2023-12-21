using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingPanel : UIMenuBase
{
    [SerializeField] private Toggle soundToggle;
    protected override void OnMenuContainerEnable()
    {
        Time.timeScale = 1;
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
    }


    protected override void OnMenuContainerDisable()
    {
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }

    public void SetToggle()
    {
        Dependencies.GameDataOperations.SetSoundStatus(soundToggle.isOn);
    }

    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.Exit:
                ChangeMenuState(MenuName.MainMenu);
                break;
        }

    }


    
}
