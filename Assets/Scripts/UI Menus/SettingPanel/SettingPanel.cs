using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingPanel : UIMenuBase
{
    [SerializeField] private Toggle soundToggle;
    protected override void OnMenuContainerEnable()
    {
        Time.timeScale = 1;
        GameEvents.GamePlayEvents.mainMenuButtonTap.Register(ButtonsOnClickExecution);
        soundToggle.isOn = Dependencies.GameDataOperations.GetSoundStatus();
    }


    protected override void OnMenuContainerDisable()
    {
        GameEvents.GamePlayEvents.mainMenuButtonTap.UnRegister(ButtonsOnClickExecution);
    }

    public void SetToggle()
    {
        if (soundToggle.isOn)
        {
            Dependencies.SoundHandler.UnMuteAll();
            Dependencies.GameDataOperations.SetSoundStatus(true);
            Dependencies.GameDataOperations.SaveData();
        }
        else
        {
            Dependencies.SoundHandler.MuteAll();
            Dependencies.GameDataOperations.SetSoundStatus(false);
            Dependencies.GameDataOperations.SaveData();
        }
            
    }

    private void ButtonsOnClickExecution(ButtonType type)
    {
        switch (type)    
        {
            case ButtonType.Exit:
                ChangeMenuState(MenuName.MainMenu);
                break;
            case ButtonType.RevokeConsent:
                Dependencies.GameDataOperations.SetConsent(false);
                SceneManager.LoadScene(SceneName.Splash.ToString());
                break;
        }
    }
}
