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
        shadowToggle.onValueChanged.AddListener(SetShadows);
    }
    
    private void Start()
    {
        Initailize();
        updateToggleButtons();
    }

    private void updateToggleButtons()
    {
        musicToggle.isOn = Dependencies.GameDataOperations.GetSoundStatus();

    }
    
    public void SetMusic(bool status)
    {
        print(status);
        Dependencies.GameDataOperations.SetSoundStatus(status);
        Dependencies.GameDataOperations.SaveData();
        
    }

    public void SetHaptic(bool status)
    {
       // Dependencies.GameDataOperations.SetHapticSound(status);
        Dependencies.GameDataOperations.SaveData();
    }

    public void SetShadows(bool status)
    {
       // Dependencies.GameDataOperations.SetShadow(status);
        Dependencies.GameDataOperations.SaveData();
    }

    
    public void onCloseButtonTap()
    {
        ChangeMenuState(MenuName.MainMenu);
    }
}
