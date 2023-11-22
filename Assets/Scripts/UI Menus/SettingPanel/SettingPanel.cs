using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingPanel : UIMenuBase
{
    [SerializeField] private Button m_CloseBtnTap;
    
    public Toggle musicToggle;
    public Toggle hapticToggle;
    public Toggle shadowToggle;

    private void Awake()
    {
        
    }

    public void Initailize()
    {
        m_CloseBtnTap.onClick.AddListener(onCloseButtonTap);
        musicToggle.onValueChanged.AddListener(SetMusic);
        hapticToggle.onValueChanged.AddListener(SetHaptic);
        shadowToggle.onValueChanged.AddListener(SetShadow);
    }
    
    private void Start()
    {
        Initailize();
        updateToggleButtons();
    }

    private void updateToggleButtons()
    {
        musicToggle.isOn = SaveLoadData.GameData.sound;
        hapticToggle.isOn = SaveLoadData.GameData.haptic;
        shadowToggle.isOn = SaveLoadData.GameData.shadow;
    }
    
    public void SetMusic(bool status)
    {
        print(status);
        SaveLoadData.GameData.sound = status;
        SaveLoadData.SaveData();
    }

    public void SetHaptic(bool status)
    {
        SaveLoadData.GameData.haptic = status;
        SaveLoadData.SaveData();
    }

    public void SetShadow(bool status)
    {
        SaveLoadData.GameData.shadow = status;
        SaveLoadData.SaveData();
    }

    
    public void onCloseButtonTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
