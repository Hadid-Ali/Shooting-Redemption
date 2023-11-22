using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : UIMenuBase
{
    [SerializeField] private Button m_PlayButon;
    [SerializeField] private Button m_DailyGoal;
    [SerializeField] private Button m_SettingPanel;
    [SerializeField] private Button m_QuitPanel;
    [SerializeField] private Button m_CharacterPanel;
    [SerializeField] private Button m_GunPanel;
    [SerializeField] private Button m_EpisodeSelectionPanel;

    [SerializeField]private Text CoinTxt;
    
    private void Start()
    {
        Initialize();
        updateCoins();
    }
    
    public void updateCoins()
    {
        CoinTxt.text = SaveLoadData.GameData.m_Coins.ToString();
    }

    private void Initialize()
    {
        m_PlayButon.onClick.AddListener(OnPlayBtnTap);
        m_DailyGoal.onClick.AddListener(OnDailyBtnTap);
        m_SettingPanel.onClick.AddListener(OnSettingBtnTap);
        m_QuitPanel.onClick.AddListener(OnQuitBtnTap);
        m_CharacterPanel.onClick.AddListener(OnCharacterBtnTap);
        m_GunPanel.onClick.AddListener(OnGunBtnTap);
        m_EpisodeSelectionPanel.onClick.AddListener(OnEpisodeSelectionBtnTap);
    }

    private void OnPlayBtnTap()
    {
        ChangeMenuState(MenuName.GamePlay);
    }
    
    private void OnDailyBtnTap()
    {
        ChangeMenuState(MenuName.DailyGoals);
    }

    private void OnSettingBtnTap()
    {
        ChangeMenuState(MenuName.SettingsMenu);
    }
    
    private void OnQuitBtnTap()
    {
        ChangeMenuState(MenuName.QuitPanel);
    }
    
    private void OnCharacterBtnTap()
    {
        ChangeMenuState(MenuName.CharacterShop);
    }
    
    private void OnGunBtnTap()
    {
        ChangeMenuState(MenuName.GunShop);
    }
    
    private void OnEpisodeSelectionBtnTap()
    {
        ChangeMenuState(MenuName.EpisodeSelection);
    }
}
